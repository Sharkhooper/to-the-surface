using UnityEngine;
using UnityEngine.Events;

public class InteractibleTrigger : MonoBehaviour, IInteractible {

	[SerializeField] private Orientation orientation;

	[SerializeField] private UnityEvent onInteract;

	public void Interact(PlayerActor source) {
		if (source.Orientation == orientation) {
			onInteract.Invoke();
		}
	}
}
