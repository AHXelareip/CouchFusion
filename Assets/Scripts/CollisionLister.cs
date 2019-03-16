using UnityEngine;
using System.Collections.Generic;

public class CollisionLister : MonoBehaviour
{
	public List<Collider> currentCollisions;
	
	void Awake()
	{
		currentCollisions = new List<Collider>();
	}

	void Update()
	{
		for (int i = 0; i < currentCollisions.Count; ++i)
		{
			if (currentCollisions[i] == null)
			{
				currentCollisions.RemoveAt(i);
				--i;
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		currentCollisions.Add(coll.collider);
	}

	void OnCollisionExit(Collision coll)
	{
		currentCollisions.RemoveAll(current => current == coll.collider);
	}
}
