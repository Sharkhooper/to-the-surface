using UnityEngine;

public class ProjektilController : MonoBehaviour, IResetable {

	public bool ignoreEnviroment;

	private const float MAX_LIFETIME = 15.0f;

	private float lifetime;

	private void Update() {
		if (lifetime > MAX_LIFETIME) {
			Destroy(gameObject);
		} 

		lifetime += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// Don't destroy on contact with enviroment if set to ignore
		if (other.gameObject.layer == 12 && ignoreEnviroment) return;

		Destroy(gameObject);
	}

	public void ResetToLevelBegin() {
		Destroy(gameObject);
	}
}
