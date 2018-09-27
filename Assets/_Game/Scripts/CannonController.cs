using UnityEngine;

public class CannonController : MonoBehaviour {
	public GameObject cannonBarrel;
	public GameObject fire;
	private GameObject currentFire;
	private float cooldown;

	[SerializeField] private float shotsPerSeconds;
	[SerializeField] private float speed = 10f;
	[SerializeField] private Animator anim;

	void FixedUpdate() {
		if (cooldown <= 0) {
			anim.SetTrigger("Fire");
		} else {
			cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
		}
	}

	public void Fire() {
		currentFire = Instantiate(fire, cannonBarrel.transform.position, cannonBarrel.transform.rotation);
		cooldown = 1f / (shotsPerSeconds);
		// Drehung
		currentFire.transform.rotation = Quaternion.Euler(0, 0, 90) * cannonBarrel.transform.rotation;
		// Flugrichtung
		currentFire.GetComponent<Rigidbody>().velocity = cannonBarrel.transform.right * speed;
	}
}
