using System.Collections.Generic;
using UnityEngine;

public class ChallagerController : MonoBehaviour {
	private PlayerActor pa;
	[SerializeField] private float waitTime;

	private readonly Queue<Vector3> oldPosition = new Queue<Vector3>();
	private readonly Queue<Quaternion> oldRotation = new Queue<Quaternion>();

	[SerializeField] private Collider2D col;
	[SerializeField] private Animator ani;

	
	private void Start() {

		pa=FindObjectOfType<PlayerActor>();
		
		waitTime = 2.0f;
		col.enabled = false;
		ani.enabled = false;

	}

	private void FixedUpdate() {
		if (waitTime <= 0f) {
			oldPosition.Enqueue(pa.transform.position);
			oldRotation.Enqueue(pa.transform.rotation);

			
			transform.position = oldPosition.Dequeue();
			transform.rotation = oldRotation.Dequeue();
			
			col.enabled = true;
			ani.enabled = true;
			
		}
		else {
			waitTime -= Time.deltaTime;
			oldPosition.Enqueue(pa.transform.position);
			oldRotation.Enqueue(pa.transform.rotation);
		}
		
	}
}
