using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class CFFusedPlayer : MonoBehaviour
{
	public float jumpSyncTime;
	public float jumpSpeed;
	
	public List<CFPlayer> fusedPlayers = new List<CFPlayer>();
	public bool canJump;

	public Rigidbody rig;
	public GameObject visual;
    
    public Animator playerAnimator;
    public CFPlayerAnimController animatorController;
    public CollisionSignal collisionSignal;

    void Start()
	{
		fusedPlayers[0].fused = true;
		fusedPlayers[1].fused = true;
		visual.transform.position = (fusedPlayers[0].soloGo.transform.position + fusedPlayers[1].soloGo.transform.position) / 2;

        collisionSignal.collisionEnter += OnSignalCollisionEnter;
        collisionSignal.collisionExit += OnSignalCollisionExit;
        collisionSignal.collisionStay += OnSignalCollisionStay;
        animatorController.OnEndJump += OnEndJump;
    }

    private void OnDestroy()
    {
        collisionSignal.collisionEnter -= OnSignalCollisionEnter;
        collisionSignal.collisionExit -= OnSignalCollisionExit;
        collisionSignal.collisionStay -= OnSignalCollisionStay;
        animatorController.OnEndJump -= OnEndJump;

        CFFusionManager.Instance.fusedPlayers.Clear();
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
					fusedPlayers[playerIdx].soloGo.transform.position = visual.transform.position + Vector3.up * playerIdx * 2.0f;
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

        if (move > 0)
        {
            visual.transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else if (move < 0)
        {
            visual.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }


        bool jump = true;
		foreach (CFPlayer fusedPlayer in fusedPlayers)
		{
			if (Time.time - fusedPlayer.jumpTime > jumpSyncTime)
			{
				jump = false;
				break;
			}
		}

		//canJump = rig.velocity.magnitude < 0.1f;
		
		if (jump && canJump)
		{
			foreach (CFPlayer fusedPlayer in fusedPlayers)
			{
				fusedPlayer.jumpTime = -10.0f;
			}

			Jump();
		}

        if (canJump == false)
        {
            AnimatorTransitionInfo transitionInfo = playerAnimator.GetAnimatorTransitionInfo(0);

            if (transitionInfo.duration == 0)
            {
                visual.transform.position += jumpSpeed * move * Time.deltaTime * Vector3.right;
            }
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetFloat("Speed", 1.0f);
        }
        else
        {
            playerAnimator.SetBool("Walk", Mathf.Abs(move) > 0.001f);
            playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        }
    }

	void Jump()
	{
        //rig.AddForce(Vector3.up * jumpSpeed);
        playerAnimator.SetTrigger("Jump");
        canJump = false;
    }

    private void OnSignalCollisionEnter(CollisionSignal signal, Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            canJump = true;
        }
    }

    private void OnSignalCollisionStay(CollisionSignal signal, Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            rig.useGravity = false;
        }
    }

    private void OnSignalCollisionExit(CollisionSignal signal, Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            //rig.useGravity = true;
        }
    }

    public void OnEndJump()
    {
        playerAnimator.SetTrigger("EndJump");
        rig.useGravity = true;
    }
}
