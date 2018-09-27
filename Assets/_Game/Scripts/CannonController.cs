using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
	public GameObject cannonBarrel;
	public GameObject firePrefab;
	private GameObject currentFire;
	public float fireRate = 10f;
	private float fireWait;
	public float speed = 10f;
	
	// Use this for initialization
	void Start ()
	{
		fireWait = 1f / (fireRate/10f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (fireWait <= 0)
		{
			currentFire = Instantiate(firePrefab, cannonBarrel.transform.position, cannonBarrel.transform.rotation);
			fireWait = 1f / (fireRate / 10f);
			// Drehung
			currentFire.transform.rotation = Quaternion.Euler(0, 0, 90) * cannonBarrel.transform.rotation;
			// Flugrichtung
			currentFire.GetComponent<Rigidbody>().velocity = cannonBarrel.transform.right * speed;

		}
		else
		{
			fireWait -= Time.deltaTime;
		}
	}
}
