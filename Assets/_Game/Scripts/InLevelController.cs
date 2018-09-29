using System.Collections;
using UnityEngine;

public class InLevelController : MonoBehaviour {
	private void Start() {
		if (GameManager.Instance.IsChallengerModeEnabled) {
			Instantiate(Resources.Load<GameObject>("ChallengerModus"));
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!GameManager.Instance.PauseMenuActive()) {
				GameManager.Instance.PauseLevel();
			} else {
				GameManager.Instance.ContinueLevel();
			}
		}
	}

	public void LevelFinished() {
		StartCoroutine(LevelEndRoutine());
	}

	private IEnumerator LevelEndRoutine() {
		PlayerActor player = FindObjectOfType<PlayerActor>();
		Animator anim = player.GetComponent<Animator>();
		player.LevelEnd();

		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

		GameManager.Instance.LevelFinished();
	}
}
