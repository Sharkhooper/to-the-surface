using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LaserController : MonoBehaviour {

	[SerializeField] private Sprite active;
	[SerializeField] private Sprite inactive;

	[SerializeField] private List<SpriteRenderer> endpoints;
	[SerializeField] private List<SpriteRenderer> laserRenderer;
	[SerializeField] private List<Collider2D> lasercollider;

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
	}

    [Button]
    public void ToggleState() {
        if (laserRenderer[0] == null) return;

        bool state = laserRenderer[0].enabled;

        SetState(!state);
    }
}
