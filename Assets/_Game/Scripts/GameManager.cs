using System.Collections;
using System.Collections.Generic;
using Soraphis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;






public class GameManager : SingletonBehaviour<GameManager>
{
	
	private int currentLevel = 1; //Current level number, expressed in game as "Day 1".
	public int HighestLevel { get; private set; }
	private int lastLevel;		//Die Anzahl aller exestierenden Level (Maximale ANzahl Level im Speil)


	private GameObject PauseMenu =null ;		//Damit wird das PauseMenu gespeichert
	public bool inLevel = false;
	

	private LoadSave ls = new LoadSave();
	

	//Awake is always called before any Start functions

	private void Awake()
	{
		HighestLevel = 1;
		
		lastLevel = SceneManager.sceneCountInBuildSettings-2;


		if (ls.DoesSaveFileExist())
		{
			int tmp = 1;

			ls.Load(out currentLevel, out tmp);
			HighestLevel = tmp;

		}

	}

	void Update()
	{
		/*
		if (inLevel)
		{
			Debug.Log("In Level");
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.Log("Escape");
				if( !(PauseMenu.activeSelf))
				{
					PauseLevel();
				}
				else
				{
					ContinueLevel();
				}
			}
			Debug.Log("InLevel= " + inLevel);
		}
		*/
	}

	//	Update nicht benötigt da alle veränderungen über Methodenaufrufe laufen

	public bool PauseMenuActive()
	{
        return inLevel; 
	}
	
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

		inLevel = true;
		SceneManager.LoadScene(load,LoadSceneMode.Single);
		
		
	}

	public void ContinueGame() //Continue im Hauptmenu
	{

		LoadLevel(currentLevel);

	}


	


	public void CloseGame()		// beendet das Spiel
	{
		Application.Quit();
		
	}


	public void LevelFinished()		// wenn man ein Level beendet hat
	{
		if(currentLevel<lastLevel)
		currentLevel++;

		if (currentLevel > HighestLevel)
			HighestLevel++;

		int tmp = HighestLevel;
		ls.Safe(currentLevel,tmp);
		
		LoadLevel(currentLevel);

	}


	public void ResetLevel()		// Neustarte bzw. neu lade das Leven/ die scene
	{
		LoadLevel(currentLevel);
		
	}

	public void PauseLevel()		// pausiert das lvl indem das Gameobject PauseMenu aktiv wird
	{
		if (PauseMenu==null)				// erstellt das PauseMenu wenn es noch keines gibt
		{
			PauseMenu=Instantiate(Resources.Load("PauseMenu",typeof(GameObject)))as GameObject;
		}
		
		Time.timeScale = 0.0f;
		PauseMenu.SetActive(true);
		
		
		
	}

	public void ContinueLevel()		// weiter button im PauseMenu ; Pause Menu wird inaktiv
	{
		Time.timeScale = 1.0f;
		PauseMenu.SetActive(false);

	}

	public void GoMainMenu()		// Das Lvl wird beendet und man geht ins Hauptmenu
	{
		
		
		
		Destroy(PauseMenu);			// Da im hauptmenu kein PauseMenu gebraucht wird wird es zerstört und wieder erstellt wenn man ein lvl startet
		PauseMenu = null;



		inLevel = false;
		SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);		// aufruf des Mainmenu
		
		
		
		
	}

	public void ResetProgress()		// Man löscht seinen gesamten Fortschritt und hat wieder keine Lvl freigeschaltet
	{
		currentLevel = 1;
		HighestLevel = 1;

	}

	


	
	
	
}