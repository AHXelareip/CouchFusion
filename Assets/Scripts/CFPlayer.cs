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
	public Animator playerAnimator;
    public CFPlayerAnimController animatorController;
    public CollisionSignal collisionSignal;

    private void Start()
    {
        collisionSignal.collisionEnter += OnSignalCollisionEnter;
        collisionSignal.collisionExit += OnSignalCollisionExit;
        collisionSignal.collisionStay += OnSignalCollisionStay;
        animatorController.OnEndJump += OnEndJump;
    }

    private void OnDestroy()
    {
        collisionSignal.collisionEnter -= OnSignalCollisionEnter;
        collisionSignal.collisionExit -= OnSignalCollisionExit;
        collisionSignal.collisionStay += OnSignalCollisionStay;
    }

    void Update ()
	{
		unfuse = false;
		soloGo.SetActive(fused == false);
        
        currentSpeed = Input.GetAxis("P" + playerId + "_Horizontal") * speed;

        if (currentSpeed > 0)
		{
			soloGo.transform.rotation = Quaternion.LookRotation(Vector3.back);
		}
		else if (currentSpeed < 0)
		{
			soloGo.transform.rotation = Quaternion.LookRotation(Vector3.forward);
		}
		
        if (fused == false)
        {
            if (canJump == false)
            {
                currentSpeed = Input.GetAxis("P" + playerId + "_Horizontal") * jumpSpeed;
                AnimatorTransitionInfo transitionInfo = playerAnimator.GetAnimatorTransitionInfo(0);
                
                if (transitionInfo.duration == 0)
                {
                    soloGo.transform.position += currentSpeed * Time.deltaTime * Vector3.right;
                }
                playerAnimator.SetBool("Walk", false);
            }
            else
            {
                playerAnimator.SetBool("Walk", Mathf.Abs(currentSpeed) > 0.001f);
                playerAnimator.SetFloat("Speed", Mathf.Abs(currentSpeed));
            }
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
            playerAnimator.SetTrigger("Jump");
			canJump = false;
		}
	}

    private void OnSignalCollisionEnter(CollisionSignal signal,  Collision other)
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
