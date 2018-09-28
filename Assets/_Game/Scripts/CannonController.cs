using UnityEngine;

public class CannonController : MonoBehaviour, IResetable {
	[SerializeField] private GameObject cannonBarrel;
	[SerializeField] private GameObject fire;
	[SerializeField] private float fireDelay;
	[SerializeField] private float speed = 10f;
	[SerializeField] private Animator anim;
	[SerializeField] private Animator animRadVorne;
	[SerializeField] private Animator animRadHinten;

	private void Start()
	{
		InvokeRepeating("FireCannon", 0.0f, fireDelay);
	}
	
	public void FireCannon() {
		anim.SetTrigger("Fire");
		
		GameObject obj = Instantiate(fire, cannonBarrel.transform.Find("FirePoint").position, cannonBarrel.transform.rotation);
		// Drehung
		obj.transform.rotation = Quaternion.Euler(0, 0, 90) * cannonBarrel.transform.rotation;
		obj.transform.localScale = transform.lossyScale;
		// Flugrichtung
		obj.GetComponent<Rigidbody2D>().velocity = cannonBarrel.transform.right * speed;
	}

	public void ResetToLevelBegin() {
		
	}
}
