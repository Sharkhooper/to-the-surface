using UnityEngine;

public class PauseMenuController : MonoBehaviour {
	public void GoMainMenu() {
		GameManager.Instance.GoMainMenu();
	}

	public void ContinueLevel() {
		GameManager.Instance.ContinueLevel();
	}

	public void ResetLevel() {
		GameManager.Instance.ResetLevel();
	}
}
