using UnityEngine;

public class CannonController : MonoBehaviour {
	[SerializeField] private GameObject cannonBarrel;
	[SerializeField] private GameObject fire;
	[SerializeField] private float shotsPerSeconds;
	[SerializeField] private float speed = 10f;
	[SerializeField] private Animator anim;
	[SerializeField] private Animator animRadVorne;
	[SerializeField] private Animator animRadHinten;

	private float cooldown;

	private void FixedUpdate () {
		if (cooldown <= 0) {
			anim.SetTrigger("Fire");
			animRadVorne.SetTrigger("Fire");
			animRadHinten.SetTrigger("Fire");
		} else {
			cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
		}
	}

	public void Fire() {
		GameObject obj = Instantiate(fire, cannonBarrel.transform.Find("FirePoint").position, cannonBarrel.transform.rotation);
		cooldown = 1f / (shotsPerSeconds);
		// Drehung
		obj.transform.rotation = Quaternion.Euler(0, 0, 90) * cannonBarrel.transform.rotation;
		obj.transform.localScale = transform.lossyScale;
		// Flugrichtung
		obj.GetComponent<Rigidbody2D>().velocity = cannonBarrel.transform.right * speed;
	}
}
