using System.Collections.Generic;
using UnityEngine;

public class ChallagerController : MonoBehaviour {
	private PlayerActor pa;
	public float waitTime;

	private readonly Queue<Vector3> oldPosition = new Queue<Vector3>();

	private void Start() {
		pa = GetComponent<PlayerActor>();

		waitTime = 2.0f;
	}

	private void FixedUpdate() {
		if (waitTime <= 0f) {
			oldPosition.Enqueue(pa.transform.position);

			transform.position = oldPosition.Dequeue();
		}
		else {
			waitTime -= Time.deltaTime;
			oldPosition.Enqueue(pa.transform.position);
		}
	}
}
