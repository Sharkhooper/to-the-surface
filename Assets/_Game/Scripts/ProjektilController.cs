using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjektilController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.name);
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
