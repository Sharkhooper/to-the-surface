using UnityEngine;

public class ProjektilController : MonoBehaviour {

	private const float MAX_LIFETIME = 15.0f;

	private float lifetime;

	private void Update() {
		if (lifetime > MAX_LIFETIME) {
			Destroy(gameObject);
		}

		lifetime += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Destroy(gameObject);
	}
}
