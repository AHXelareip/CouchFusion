using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFFusionManager : MonoBehaviour
{
	public float fuseSyncTime;

	public GameObject fusedPrefab;
	
	public List<CFFusedPlayer> fusedPlayers;
	public List<CFPlayer> players;

	void Update()
	{
		List<CFPlayer> readyToFuse = new List<CFPlayer>();
		foreach (CFPlayer player in players)
		{
			if (player.fused == false && (Time.time - player.fuseTime < fuseSyncTime))
			{
				readyToFuse.Add(player);
			}
		}

		if (readyToFuse.Count == 2)
		{
			GameObject fusedPlayerGo = Instantiate(fusedPrefab);
;
			CFFusedPlayer fusedPlayer = fusedPlayerGo.GetComponent<CFFusedPlayer>();
			
			fusedPlayer.fusedPlayers.Add(readyToFuse[0]);
			fusedPlayer.fusedPlayers.Add(readyToFuse[1]);
			readyToFuse[0].fused = true;
			readyToFuse[1].fused = true;
			
			readyToFuse[0].soloGo.SetActive(false);
			readyToFuse[1].soloGo.SetActive(false);
		}
	}
}
