using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleLever : MonoBehaviour, IInteractible, IResetable {
	[System.Serializable]
	private class BoolEvent : UnityEvent<bool> { }

	[SerializeField] private Orientation orientation;

	[SerializeField] private bool isActive;

	[SerializeField] private SpriteRenderer render;
	[SerializeField] private Sprite active;
	[SerializeField] private Sprite inactive;

	[SerializeField] private BoolEvent onInteract;
	[SerializeField] private BoolEvent onInteractInversed;

	private bool state;

	private void Awake() {
		ResetToLevelBegin();
	}

    public void Interact(PlayerActor source) {
		if (source.Orientation == orientation) {
			state = !state;
			onInteract.Invoke(state);
            onInteractInversed.Invoke(!state);

			UpdateSprite();
		}
	}

	private void UpdateSprite() {
		render.sprite = state ? active : inactive;
	}

	public void ResetToLevelBegin() {
		state = isActive;
		UpdateSprite();
	}
}
