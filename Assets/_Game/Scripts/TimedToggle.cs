using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedToggle : MonoBehaviour {
	[System.Serializable]
	private class BoolEvent : UnityEvent<bool> { }

	[SerializeField] private bool isActive;

	[SerializeField] private float onDuration;
	[SerializeField] private float offDuration;

	[SerializeField] private BoolEvent onStateChange;

	private float time;

	private bool state;

	private void Start() {
		state = isActive;
	}

	private void Update() {
		if (state && time >= onDuration) {
			time -= onDuration;
			state = false;

			onStateChange.Invoke(false);
		} else if (!state && time >= offDuration) {
			time -= offDuration;
			state = true;

			onStateChange.Invoke(true);
		}

		time += Time.deltaTime;
	}
}
