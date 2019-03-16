using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFMiddleTracker : MonoBehaviour
{
    public List<Transform> targets;
    
	void Update ()
    {
        Vector3 acc = Vector3.zero;
        foreach (Transform t in targets)
        {
            acc += t.position;
        }

        transform.position = acc / targets.Count;
	}
}
