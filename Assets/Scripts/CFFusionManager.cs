using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FUSE_MODE
{
    MIDDLE,
    CURSOR,
    DIRECTION
}

public class CFFusionManager : MonoBehaviour
{
    private static CFFusionManager _instance;
    public static CFFusionManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool showFuse;
    public FUSE_MODE fuseMode;
    public bool needLOS;
    public bool freezeFuse;
    public float fuseDist;

    public float cursorSpeed;

    public bool fuseKeepPress;
    public float fuseSyncTime;
    public float fuseSyncCD;

    public GameObject fusedPrefab;
	
	public List<CFFusedPlayer> fusedPlayers;
	public List<CFPlayer> players;
    public List<CFPlayer> readyToFuse;

    private void Start()
    {
        _instance = this;
    }

    void Update()
	{
        //DrawDebug();

		readyToFuse = new List<CFPlayer>();
		foreach (CFPlayer player in players)
		{
			if (player.fused == false && player.IsWaitingForFuse())
			{
				readyToFuse.Add(player);
			}
		}

        // Shall we fuse?

		if (CanFuse() && ShouldFuseNow())
		{
			GameObject fusedPlayerGo = Instantiate(fusedPrefab);
;
			CFFusedPlayer fusedPlayer = fusedPlayerGo.GetComponent<CFFusedPlayer>();
			
			fusedPlayer.fusedPlayers.Add(readyToFuse[0]);
			fusedPlayer.fusedPlayers.Add(readyToFuse[1]);
            fusedPlayers.Add(fusedPlayer);

			readyToFuse[0].fused = true;
			readyToFuse[1].fused = true;
			
			readyToFuse[0].soloGo.SetActive(false);
			readyToFuse[1].soloGo.SetActive(false);
		}
	}

    //private void DrawDebug()
    //{
    //    if (showDebug == false)
    //    {
    //        foreach (GameObject go in debugCursors)
    //        {
    //            go.transform.position = Vector3.zero;
    //        }
    //        return;
    //    }
    //
    //    for (int playerIdx = 0; playerIdx < 2; ++playerIdx)
    //    {
    //        Debug.
    //        debugCursors[playerIdx].transform.position = GetFusePosition(playerIdx);
    //    }
    //
    //    Debug.DrawLine(players[0].position, players[1].position);
    //}

    private void OnDrawGizmos()
    {
        if (_instance == null)
            return;
        if (players[0].fused)
            return;
        Gizmos.color = CanFuse() ? Color.green : Color.red;
        Gizmos.DrawLine(players[0].position, players[1].position);
        foreach (Vector3 position in GetFusePositions())
        {
            Gizmos.DrawSphere(position, 0.25f);
        }
    }

    private Vector3[] GetFusePositions()
    {
        Vector3[] result = new Vector3[2];
        for (int playerIdx = 0; playerIdx < 2; ++playerIdx)
        {
            result[playerIdx] = GetFusePosition(playerIdx);
        }
        return result;
    }

    private Vector3 GetFusePosition(int playerIdx)
    {
        CFPlayer player = players[playerIdx];
        CFPlayer otherPlayer = players[1 - playerIdx];
        switch (fuseMode)
        {
            case FUSE_MODE.MIDDLE:
                return (player.position + otherPlayer.position) / 2.0f;
            case FUSE_MODE.CURSOR:
                if (player.IsWaitingForFuse() == false)
                    break;
                if (cursorSpeed > 0.0f)
                {
                    return player.position + (otherPlayer.position - player.position).normalized * Mathf.Min(cursorSpeed * (Time.time - player.fuseTime), Vector3.Distance(otherPlayer.position, player.position));
                }
                return player.position + (otherPlayer.position - player.position) * (Time.time - player.fuseTime) / fuseSyncTime;
        }
        return Vector3.zero;
    }

    private bool CanFuse()
    {
        if (fuseDist > 0.0f && Vector3.Distance(players[0].position, players[1].position) > fuseDist)
        {
            return false;
        }
        if (needLOS)
        {
            RaycastHit[] hits = Physics.RaycastAll(players[0].position, players[1].position - players[0].position, Vector3.Distance(players[0].position, players[1].position));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject != players[0].soloGo && hit.transform.gameObject != players[1].soloGo)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool ShouldFuseNow()
    {
        if (readyToFuse.Count != 2)
        {
            return false;
        }
        switch (fuseMode)
        {
            case FUSE_MODE.MIDDLE:
                return true;
            case FUSE_MODE.CURSOR:
                return Vector3.Distance(GetFusePosition(0), players[0].position) >= Vector3.Distance(GetFusePosition(1), players[0].position);
        }
        return false;
    }
}
