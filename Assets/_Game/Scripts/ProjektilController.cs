using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjektilController : MonoBehaviour
{
	private Rigidbody rb;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name != "Cannon" || other.gameObject.name != "CannonBarrel")
		{
			
		}
		else
		{
			Destroy(gameObject);
			PlayerActor player = other.GetComponent<PlayerActor>();
			if (player != null)
				player.Die(); 
		}
	}
	
	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
