using Soraphis;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : SingletonBehaviour<GameManager> {
	private int currentLevel = 1; //Current level number, expressed in game as "Day 1".
	public int HighestLevel { get; private set; }
	public int LastLevel { get; private set; } //Die Anzahl aller existierenden Level (Maximale Anzahl Level im Spiel)


	private GameObject PauseMenu;

	public bool IsInLevel { get; set; }

	public bool IsChallengerModeEnabled { get; set; }

	private void Awake() {
		IsChallengerModeEnabled = true;
		IsInLevel = false;
		HighestLevel = 1;

		LastLevel = SceneManager.sceneCountInBuildSettings - 1;

		if (LoadSave.DoesSaveFileExist) {
			LoadSave.Load(out currentLevel, out int tmp);
			HighestLevel = tmp;
		}
	}


	//	Update nicht benötigt da alle veränderungen über Methodenaufrufe laufen

	//lädt das ausgewählte lvl abhängig seiner Nummer
	public void LoadLevel(int lvl) {
		currentLevel = lvl;
		string load;
		if (lvl < 10) {
			load = "Level0" + lvl;
		} else {
			load = "Level" + lvl;
		}

		IsInLevel = true;
		SceneManager.LoadScene(load, LoadSceneMode.Single);

		Time.timeScale = 1.0f;
	}

	//Continue im Hauptmenu
	public void ContinueGame() {
		LoadLevel(currentLevel);
	}

	/// <summary>
	/// Exits the Application
	/// </summary>
	public void CloseGame() {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}


	public void LevelFinished() // wenn man ein Level beendet hat
	{
		if (currentLevel < LastLevel) {
			currentLevel++;
		}

		if (currentLevel > HighestLevel) {
			HighestLevel++;
		}

		int tmp = HighestLevel;
		LoadSave.Save(currentLevel, tmp);

		LoadLevel(currentLevel);
	}

	// Neustarte bzw. neu lade das Leven/ die scene
	public void ResetLevel() {
		LoadLevel(currentLevel);
	}

	// pausiert das lvl indem das Gameobject PauseMenu aktiv wird
	public void PauseLevel() {
		// Lazy Initialize pause menu
		if (PauseMenu == null) {
			PauseMenu = Instantiate(Resources.Load("PauseMenu", typeof(GameObject))) as GameObject;
		}

		Time.timeScale = 0.0f;
		PauseMenu.SetActive(true);
	}

	public bool PauseMenuActive() {
		if (PauseMenu != null) {
			return PauseMenu.activeSelf;
		}
		else {
			return false;
		}
	}

	// weiter button im PauseMenu ; Pause Menu wird inaktiv
	public void ContinueLevel() {
		Time.timeScale = 1.0f;
		PauseMenu.SetActive(false);
	}

	// Das Lvl wird beendet und man geht ins Hauptmenu
	public void GoMainMenu() {
		// Da im hauptmenu kein PauseMenu gebraucht wird wird es zerstört und wieder erstellt wenn man ein lvl startet
		Destroy(PauseMenu);
		PauseMenu = null;

		IsInLevel = false;

		// aufruf des Mainmenu
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	/// <summary>
	/// Resets Progress so no levels are unlocked
	/// </summary>
	public void ResetProgress() {
		currentLevel = 1;
		HighestLevel = 1;
		int highestLevel = HighestLevel;
		LoadSave.Save(currentLevel, highestLevel);
	}
}
