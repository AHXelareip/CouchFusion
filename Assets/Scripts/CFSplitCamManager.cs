using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFSplitCamManager : MonoBehaviour
{
    public Camera mainCam;
    public CFSplitCam splitCam1;
    public CFSplitCam splitCam2;
    public RawImage Render1;
    public RawImage Render2;

    private void Update()
    {
        Vector3 diff = splitCam2.target.transform.position - splitCam1.target.transform.position;

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, diff);

        Render1.transform.rotation = rot;
        Render2.transform.rotation = rot;
        splitCam1.transform.rotation = rot;
        splitCam2.transform.rotation = rot;
    }

}
