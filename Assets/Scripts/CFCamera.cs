using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFCamera : MonoBehaviour
{
    public GameObject cameraHolder;
    
	void Update ()
    {
        if (CFFusionManager.Instance.fusedPlayers.Count == 1)
        {
            cameraHolder.transform.position = CFFusionManager.Instance.fusedPlayers[0].transform.position;
        }
        else
        {
            cameraHolder.transform.position = (CFFusionManager.Instance.players[0].position + CFFusionManager.Instance.players[1].position) / 2.0f;
        }
    }
}
