using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFLevelInit : MonoBehaviour
{
    public Vector3[] playersPositions;

	// Use this for initialization
	void Start ()
    {
		for (int i = 0; i < CFFusionManager.Instance.players.Count; ++i)
        {
            CFFusionManager.Instance.players[i].soloGo.transform.position = playersPositions[i];
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Vector3 pos in playersPositions)
        {
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
