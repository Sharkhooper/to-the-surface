using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleTrigger : MonoBehaviour, IInteractible {

	[SerializeField] private Orientation orientation;

	[SerializeField] private UnityEvent onInteract;

	private PlayerActor player;

	private void OnTriggerEnter2D(Collider2D other) {
		player = other.GetComponent<PlayerActor>();
		if(player != null) {
			Debug.Log("Test");
			player.Interactible = this;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (player != null) {
			player.Interactible = null;
		}
	}

	public void Interact() {
		if (player.Orientation == orientation) {
			onInteract.Invoke();
		}
	}

	public void Test() {
		Debug.Log("Test");
	}
}
