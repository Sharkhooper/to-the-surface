using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LaserController : MonoBehaviour, IResetable {

	[SerializeField] private bool isActive = true;

	[SerializeField] private Sprite active;
	[SerializeField] private Sprite inactive;

	[SerializeField] private List<SpriteRenderer> endpoints;
	[SerializeField] private List<SpriteRenderer> laserRenderer;
	[SerializeField] private List<Collider2D> lasercollider;

	private bool state;

	private void Awake() {
		state = isActive;
	}

	public void SetState(bool value) {
		foreach (SpriteRenderer r in endpoints) {
			r.sprite = value ? active : inactive;
		}

		foreach (SpriteRenderer r in laserRenderer) {
			r.enabled = value;
		}

		foreach (Collider2D c in lasercollider) {
			c.enabled = value;
		}

		state = value;
	}

    [Button]
    public void ToggleState() {
		SetState(!state);
    }

	public void ResetToLevelBegin() {
		SetState(isActive);
	}
}
