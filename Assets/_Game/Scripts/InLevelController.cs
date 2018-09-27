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
		GameManager.Instance.LevelFinished();
	}
}
