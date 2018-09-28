using System.Collections.Generic;
using UnityEngine;

public class ChallagerController : MonoBehaviour {
	private PlayerActor pa;
	public float waitTime;

	private readonly Queue<Vector3> oldPosition = new Queue<Vector3>();
	private readonly Queue<Quaternion> oldRotation = new Queue<Quaternion>();

	private void Start() {

		pa=FindObjectOfType<PlayerActor>();
		
		waitTime = 2.0f;
	}

	private void FixedUpdate() {
		if (waitTime <= 0f) {
			oldPosition.Enqueue(pa.transform.position);
			oldRotation.Enqueue(pa.transform.rotation);

			transform.position = oldPosition.Dequeue();
			transform.rotation = oldRotation.Dequeue();
		}
		else {
			waitTime -= Time.deltaTime;
			oldPosition.Enqueue(pa.transform.position);
			oldRotation.Enqueue(pa.transform.rotation);
		}
		
	}
}
