using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class CFFusedPlayer : MonoBehaviour
{
	public float jumpSyncTime = 1.0f;
	public float jumpSpeed = 800.0f;
	
	public List<CFPlayer> fusedPlayers = new List<CFPlayer>();
	public bool canJump;

	public Rigidbody rig;
	public GameObject visual;

	void Start()
	{
		fusedPlayers[0].fused = true;
		fusedPlayers[1].fused = true;
		visual.transform.position = (fusedPlayers[0].soloGo.transform.position + fusedPlayers[1].soloGo.transform.position) / 2;
	}
	
	void Update ()
	{
		foreach (CFPlayer fusedPlayer in fusedPlayers)
		{
			if (fusedPlayer.unfuse)
			{
				for (int playerIdx = 0; playerIdx < fusedPlayers.Count; ++playerIdx)
				{
					fusedPlayers[playerIdx].fused = false;
					fusedPlayers[playerIdx].soloGo.transform.position = visual.transform.position + Vector3.up * playerIdx;
					fusedPlayers[playerIdx].soloGo.SetActive(true);
				}
				DestroyImmediate(gameObject);
				
				return;
			}
		}
		
		float move = 0.0f;
		foreach (CFPlayer player in fusedPlayers)
		{
			move += player.currentSpeed;
		}

		move /= fusedPlayers.Count;
		move *= 1.5f;

		transform.position += Vector3.right * move * Time.deltaTime;
		
		bool jump = true;
		foreach (CFPlayer fusedPlayer in fusedPlayers)
		{
			if (Time.time - fusedPlayer.jumpTime > jumpSyncTime)
			{
				jump = false;
				break;
			}
		}

		canJump = rig.velocity.magnitude < 0.1f;
		
		if (jump && canJump)
		{
			foreach (CFPlayer fusedPlayer in fusedPlayers)
			{
				fusedPlayer.jumpTime = -10.0f;
			}

			Jump();
		}
	}

	void Jump()
	{
		rig.AddForce(Vector3.up * jumpSpeed);
		canJump = false;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ground")
		{
			canJump = true;
		}
	}
}
