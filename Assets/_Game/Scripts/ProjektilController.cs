using UnityEngine;

public class ProjektilController : MonoBehaviour {
	
	private void OnTriggerEnter2D(Collider2D other) {
		Destroy(gameObject);
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
