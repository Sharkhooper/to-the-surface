using UnityEngine;

public class MainMenuController : MonoBehaviour {
	private void Awake() {
		GameManager.Instance.IsInLevel = false;
	}

	public void Play() {
		GameManager.Instance.ContinueGame();
	}

	public void LoadLevel(int level) {
		GameManager.Instance.LoadLevel(level);
	}

	public void Exit() {
		GameManager.Instance.CloseGame();
	}

	public void ResetProgress() {
		GameManager.Instance.ResetProgress();
	}
}
