﻿using System.Collections.Generic;
using UnityEngine;

public class ChallengerController : MonoBehaviour, IResetable {
	private PlayerActor player;
	[SerializeField] [Range(0, 15)] private float waitTime;

	private readonly Queue<Location> queue = new Queue<Location>();

	[SerializeField] private List<GameObject> children;

	private int objectsInQueueBuffer;

	private struct Location {
		public Vector3 position;
		public Quaternion rotation;
	}

	private bool IsActive => queue.Count >= objectsInQueueBuffer;

	private void Start() {
		player = FindObjectOfType<PlayerActor>();

		objectsInQueueBuffer = Mathf.CeilToInt(waitTime / Time.fixedDeltaTime);

		ResetToLevelBegin();
	}

	private void FixedUpdate() {
		queue.Enqueue(new Location {
			position = player.transform.position,
			rotation = player.transform.rotation
		});

		foreach (GameObject child in children) {
			child.SetActive(IsActive);
		}

		if (IsActive) {
			Location l = queue.Dequeue();
			transform.position = l.position;
			transform.rotation = l.rotation;
		}

	}

	public void ResetToLevelBegin() {
		queue.Clear();
		foreach (GameObject child in children) {
			child.SetActive(false);
		}
	}
}
