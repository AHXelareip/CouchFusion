using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class CFFusedPlayer : MonoBehaviour
{
	public float jumpSyncTime;
	public float jumpSpeed;
    public float fallSpeed;
	
	public List<CFPlayer> fusedPlayers = new List<CFPlayer>();
	public bool canJump;

	public Rigidbody rig;
	public GameObject visual;
    
    public Animator playerAnimator;
    public CFPlayerAnimController animatorController;
    public CollisionSignal collisionSignal;
    public CollisionLister collisionLister;
    public Collider playerCollider;

    void Start()
	{
		fusedPlayers[0].fused = true;
		fusedPlayers[1].fused = true;
		visual.transform.position = (fusedPlayers[0].soloGo.transform.position + fusedPlayers[1].soloGo.transform.position) / 2;

        RaycastHit hit;
        Physics.Raycast(visual.transform.position, Vector3.down, out hit, 5.0f);

        visual.transform.position += (5.0f - hit.distance) * Vector3.up;

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

        playerAnimator.SetBool("Falling", !canJump);
        float currentSpeed = move;
        if (canJump == false)
        {
            currentSpeed *= jumpSpeed;
            visual.transform.position += CanMove(currentSpeed * Time.deltaTime) * Vector3.right;
            visual.transform.position += fallSpeed * Time.deltaTime * Vector3.down;

            playerAnimator.SetBool("Walk", Mathf.Abs(currentSpeed) > 0.001f);
            playerAnimator.SetFloat("Speed", 1.0f);
            playerAnimator.SetFloat("StickSpeed", currentSpeed);
        }
        else
        {
            playerAnimator.SetBool("Walk", Mathf.Abs(currentSpeed) > 0.001f);
            playerAnimator.SetFloat("Speed", Mathf.Abs(currentSpeed));
        }
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
        playerAnimator.SetTrigger("Jump");
    }

    private void OnSignalCollisionEnter(CollisionSignal signal, Collision other)
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
}
