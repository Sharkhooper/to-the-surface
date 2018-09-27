using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallagerController : MonoBehaviour
{


	private PlayerActor pa;
	public float waitTime;

	private Queue<Vector3> oldPosition= new Queue<Vector3>();
	
	
	// Use this for initialization
	void Start () {
		
		pa= GetComponent<PlayerActor>();

		waitTime = 2.0f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		

			if (waitTime <= 0f)
			{
				oldPosition.Enqueue(pa.transform.position);

				transform.position = oldPosition.Dequeue();
			}
			else
			{
				waitTime -= Time.deltaTime;
				oldPosition.Enqueue(pa.transform.position);
			}

		

	}
}
