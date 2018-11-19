using UnityEngine;
using System;

public class CollisionSignal : MonoBehaviour
{
	public event Action<CollisionSignal, Collision> collisionEnter;
    public event Action<CollisionSignal, Collision> collisionExit;
    public event Action<CollisionSignal, Collision> collisionStay;
    public event Action<CollisionSignal, Collision2D> collisionEnter2D;
	public event Action<CollisionSignal, Collision2D> collisionExit2D;

    void OnCollisionEnter(Collision coll)
    {
        if (collisionEnter != null)
        {
            collisionEnter(this, coll);
        }
    }

    void OnCollisionExit(Collision coll)
    {
        if (collisionExit != null)
        {
            collisionExit(this, coll);
        }
    }

    void OnCollisionStay(Collision coll)
    {
        if (collisionStay != null)
        {
            collisionStay(this, coll);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
	{
		if (collisionEnter2D != null)
		{
			collisionEnter2D(this, coll);
		}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (collisionExit2D != null)
		{
			collisionExit2D(this, coll);
		}
	}
}
