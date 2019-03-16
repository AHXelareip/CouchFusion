using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSplitCam : MonoBehaviour
{
    public Camera cam;
    public Transform aligner;
    public Transform target;
    public Vector3 offset;


    private void Start()
    {
        if (offset == Vector3.zero)
            offset = transform.position - target.position;
    }

    void Update ()
    {
        transform.position = target.position + transform.rotation * offset + Vector3.up;
	}
}
