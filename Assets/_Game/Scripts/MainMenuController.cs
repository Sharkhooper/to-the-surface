using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

	[SerializeField] private Toggle modeBool;
	
	
	private void Awake() {
		GameManager.Instance.IsInLevel = false;
	}


	private void Start()
	{

		
		modeBool.isOn=GameManager.Instance.IsChallengerModeEnabled;


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

	public void SetChallengerOn()
	{
		GameManager.Instance.IsChallengerModeEnabled = true;
	}

	public void SetChallengerOff()
	{
		GameManager.Instance.IsChallengerModeEnabled = false;
		
	}

	public void ToggleChallenger()
	{
		GameManager.Instance.IsChallengerModeEnabled = modeBool.isOn;
		GameManager.Instance.SaveData();
	}

}
