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

    public float height;
    public float fallSpeed;
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
    public CollisionLister collisionLister;
    public Collider playerCollider;

    public Vector3 position
    {
        get
        {
            return soloGo.transform.position + Vector3.up;
        }
    }

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
        animatorController.OnEndJump -= OnEndJump;
    }

    void Update ()
	{
		unfuse = false;
		soloGo.SetActive(fused == false);
        
        currentSpeed = Input.GetAxis("P" + playerId + "_Horizontal") * speed;

        if (ShouldFreeze() == false)
        {
            if (currentSpeed > 0)
            {
                soloGo.transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            else if (currentSpeed < 0)
            {
                soloGo.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }

        canJump = CanJump();

        if (fused == false)
        {
            playerAnimator.SetBool("Falling", !canJump);
            if (canJump == false)
            {
                currentSpeed = Input.GetAxis("P" + playerId + "_Horizontal") * jumpSpeed;
                
                if (ShouldFreeze() == false)
                {
                    soloGo.transform.position += CanMove(currentSpeed * Time.deltaTime) * Vector3.right;
                }

                playerAnimator.SetBool("Walk", Mathf.Abs(currentSpeed) > 0.001f);
                playerAnimator.SetFloat("Speed", ShouldFreeze() ? 0.0f : 1.0f);
                playerAnimator.SetFloat("StickSpeed", currentSpeed);
            }
            else
            {
                playerAnimator.SetBool("Walk", Mathf.Abs(currentSpeed) > 0.001f);
                playerAnimator.SetFloat("Speed", ShouldFreeze() ? 0.0f : Mathf.Abs(currentSpeed));
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
            if (FuseCD() == 0.0f)
            {
                fuseTime = Time.time;
            }
		}

		unfuse = Input.GetButtonDown("P" + playerId + "_Unfuse");
    }

    private float CanMove(float target)
    {
        float result = target;
        foreach (Collider coll in collisionLister.currentCollisions)
        {
            // Hit above or below
            if (Mathf.Abs(coll.bounds.center.y - playerCollider.bounds.center.y) > (coll.bounds.extents.y + playerCollider.bounds.extents.y))
            {
                continue;
            }
            // Hit behind
            if (target * (coll.bounds.center.x - playerCollider.bounds.center.x) < 0)
            {
                continue;
            }
            if (target > 0)
            {
                result = Mathf.Min(result, (coll.bounds.center.x - playerCollider.bounds.center.x) - (coll.bounds.extents.x + playerCollider.bounds.extents.x));
            }
            else
            {
                result = Mathf.Max(result, (coll.bounds.center.x - playerCollider.bounds.center.x) + (coll.bounds.extents.x + playerCollider.bounds.extents.x));
            }
        }
        return result;
    }

    private bool CanJump()
    {
        foreach (Collider coll in collisionLister.currentCollisions)
        {
            // Hit above or below
            if (Mathf.Abs(coll.bounds.center.x - playerCollider.bounds.center.x) > (coll.bounds.extents.x + playerCollider.bounds.extents.x))
            {
                continue;
            }

            if ((coll.bounds.center.x - playerCollider.bounds.center.x) - (coll.bounds.extents.x + playerCollider.bounds.extents.x) < 0)
            {
                return true;
            }
                
        }
        return false;
    }

    private void Jump()
	{
		if (fused == false)
		{
            playerAnimator.SetTrigger("Jump");
			//canJump = false;
		}
	}

    private void OnSignalCollisionEnter(CollisionSignal signal,  Collision other)
	{
		if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
		{
			//canJump = true;
		}
	}

    private void OnSignalCollisionStay(CollisionSignal signal, Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            //canJump = true;
        }
    }

    private void OnSignalCollisionExit(CollisionSignal signal, Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            //canJump = false;
        }
    }

    public void OnEndJump()
    {
        playerAnimator.SetTrigger("EndJump");
        //rig.useGravity = true;
    }

    public bool IsWaitingForFuse()
    {
        return Time.time - fuseTime < CFFusionManager.Instance.fuseSyncTime;
    }

    public bool ShouldFreeze()
    {
        return CFFusionManager.Instance.freezeFuse ? IsWaitingForFuse() : false;
    }

    public float FuseCD()
    {
        return CFFusionManager.Instance.fuseKeepPress ? 0.0f : Mathf.Max(0.0f, fuseTime + CFFusionManager.Instance.fuseSyncCD - Time.time);
    }
}
