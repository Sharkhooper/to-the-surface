using UnityEngine;

public class CannonController : MonoBehaviour, IResetable {
	[SerializeField] private GameObject cannonBarrel;
	[SerializeField] private GameObject fire;
	[SerializeField] private float fireDelay;
    [SerializeField] private float phase;
    [SerializeField] private float speed = 10f;
	[SerializeField] private Animator anim;
	[SerializeField] private Animator animRadVorne;
	[SerializeField] private Animator animRadHinten;
	[SerializeField] private bool ignoreEnviroment;

	private Coroutine co;

	private float time;

	private void Start() {
		ResetToLevelBegin();
	}

	private void FixedUpdate() {
		if (time >= fireDelay) {
			time -= fireDelay;
			Fire();
		}

		time += Time.deltaTime;
	}

	private void Fire() {
		anim.SetTrigger("Fire");
		
		GameObject obj = Instantiate(fire, cannonBarrel.transform.Find("FirePoint").position, cannonBarrel.transform.rotation);
		// Drehung
		obj.transform.rotation = Quaternion.Euler(0, 0, 90) * cannonBarrel.transform.rotation;
		obj.transform.localScale = transform.lossyScale;
		// Flugrichtung
		obj.GetComponent<Rigidbody2D>().velocity = cannonBarrel.transform.right * speed;
		obj.GetComponent<ProjektilController>().ignoreEnviroment = ignoreEnviroment;
	}

	public void ResetToLevelBegin() {
		time = phase;
	}
}
