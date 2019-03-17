using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFCamera : MonoBehaviour
{
    public GameObject cameraHolder;
    public Camera normalCam;
    public CFSplitCamManager splitCamManager;
    public float splitDist;
    public bool useSplit;
    public Canvas uiCanvas;

    void Update ()
    {
        useSplit = splitDist < Vector3.Distance(splitCamManager.splitCam1.target.transform.position, splitCamManager.splitCam2.target.transform.position);

        normalCam.enabled = !useSplit;
        splitCamManager.enabled = useSplit;

        uiCanvas.worldCamera = Camera.current;

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
