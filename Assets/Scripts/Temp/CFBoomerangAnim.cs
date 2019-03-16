using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFBoomerangAnim : MonoBehaviour
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    //public float current;
    public float Speed;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        Vector3 moveTarget = EndPoint - transform.position;
        float moveDistance = Speed * Time.deltaTime;

        float distLeft = Mathf.Max(0.0f, moveDistance - moveTarget.magnitude);

        transform.position += moveTarget.normalized * moveDistance + (StartPoint - EndPoint) * distLeft;

        if (distLeft > 0.0f)
        {
            Vector3 temp = StartPoint;
            StartPoint = EndPoint;
            EndPoint = temp;
        }
	}
}
