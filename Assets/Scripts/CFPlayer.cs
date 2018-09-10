using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFPlayer : MonoBehaviour
{
	public float playerId = 1;
	
	public float speed = 1.0f;
	public float jumpSpeed = 1.0f;

	public bool fused;

	public float currentSpeed;
	public float jumpTime;
	public float fuseTime;
	public bool unfuse;

	public bool canJump;
	
	public Rigidbody rig;
	public GameObject soloGo;
	
	void Update ()
	{
		unfuse = false;
		soloGo.SetActive(fused == false);
		
		currentSpeed = Input.GetAxis("P" + playerId + "_Horizontal") * speed;

		if (fused == false)
		{
			transform.position += Vector3.right * currentSpeed * Time.deltaTime;	
		}

		if (Input.GetButtonDown("P" + playerId + "_Jump"))
		{
			jumpTime = Time.time;
			if (fused == false && canJump)
			{
				Jump();
			}
		}

		if (Input.GetButtonDown("P" + playerId + "_Fuse"))
		{
			fuseTime = Time.time;
		}

		unfuse = Input.GetButtonDown("P" + playerId + "_Unfuse");
	}

	private void Jump()
	{
		if (fused == false)
		{
			rig.AddForce(Vector3.up * jumpSpeed);
			//canJump = false;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
		{
			canJump = true;
		}
	}
}
