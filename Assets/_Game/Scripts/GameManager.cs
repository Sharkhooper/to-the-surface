using System.Collections;
using System.Collections.Generic;
using Soraphis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;





public class GameManager : SingletonBehaviour<GameManager>
{

	public static GameManager
		instance = null; //Static instance of GameManager which allows it to be accessed by any other script.

	//private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int currentLevel = 1; //Current level number, expressed in game as "Day 1".
	private int highestLevel 	// Das höchste freigespielte Level
	{get{
			return highestLevel;
		}
		set { highestLevel = value; }
	}
	private int lastLevel;		//Die Anzahl aller exestierenden Level (Maximale ANzahl Level im Speil)


	public GameObject PauseMenuPrefab;		// Daraus wird das PauseMenu erstellt (Hilfestellung)
	public GameObject PauseMenu =null ;		//Damit wird das PauseMenu gespeichert

	//Awake is always called before any Start functions

	private void Awake()
	{
		
		SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);		// beim starten das SPiels wird das Mainmenu geöffnet
		highestLevel = 1;
		lastLevel = SceneManager.sceneCountInBuildSettings-2;
	}
	
		//	Update nicht benötigt da alle veränderungen über Methodenaufrufe laufen


	
	
	public void LoadLevel(int lvl)			//lädt das ausgewählte lvl abhängig seiner Nummer
	{

		currentLevel = lvl;
		string load;
		if (lvl < 10)
			load = "Level0" + lvl;
		else
		{
			load = "Level" + lvl;
		}


		if (PauseMenu==null)				// erstellt das PauseMenu wenn es noch keines gibt
		{
			PauseMenuPrefab= Resources.Load<GameObject>("Assets/Prefabs/PauseMenu.prefab");

			PauseMenu = Instantiate(PauseMenuPrefab);
		}

		PauseMenu.SetActive(false);			// macht PAuse menu inaktiv um es nicht über dem Spiel zu haben
		

		SceneManager.LoadScene(load,LoadSceneMode.Single);
		
		
	}

	public void ContinueGame() //Continue im Hauptmenu
	{

		LoadLevel(currentLevel);

	}


	public void Credits()		// aufruf der Credits im hauptmenu
	{
		
		SceneManager.LoadScene("Credits",LoadSceneMode.Single);
		
	}


	public void CloseGame()		// beendet das Spiel
	{
		Application.Quit();
		
	}


	public void LevelFinished()		// wenn man ein Level beendet hat
	{
		if(currentLevel<lastLevel)
		currentLevel++;

		if (currentLevel > highestLevel)
			highestLevel++;
		
		LoadLevel(currentLevel);

	}


	public void ResetLevel()		// Neustarte bzw. neu lade das Leven/ die scene
	{
		LoadLevel(currentLevel);
		
	}

	public void PauseLevel()		// pausiert das lvl indem das Gameobject PauseMenu aktiv wird
	{
		Time.timeScale = 0.0f;
		PauseMenu.SetActive(true);
		
	}

	public void ContinueLevel()		// weiter button im PauseMenu ; Pause Menu wird inaktiv
	{

		Time.timeScale = 1.0f;
		PauseMenu.SetActive(false);

	}

	public void CloseLevel()		// Das Lvl wird beendet und man geht ins Hauptmenu
	{
		Destroy(PauseMenu);			// Da im hauptmenu kein PauseMenu gebraucht wird wird es zerstört und wieder erstellt wenn man ein lvl startet
		SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
		
	}

	public void ResetProgress()		// Man löscht seinen gesamten Fortschritt und hat wieder keine Lvl freigeschaltet
	{
		currentLevel = 1;
		lastLevel = 1;

	}

	public void ToLevelSelectScene()	// Man kommt in die Level auswahlscene/Bildschrim von der aus man bestimmte level wählen kann (zu loadLevel)
	{
		
		SceneManager.LoadScene("LevelSelect",LoadSceneMode.Single);
		
	}


	
	
}