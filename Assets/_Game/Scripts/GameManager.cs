﻿using System;
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
	public int LastLevel{ get; private set; }	//Die Anzahl aller existierenden Level (Maximale Anzahl Level im Spiel)


	private GameObject PauseMenu =null ;		//Damit wird das PauseMenu gespeichert
	public bool inLevel { get; set; }
	
	
	

	private LoadSave ls = new LoadSave();
	

	//Awake is always called before any Start functions

	private void Awake()
	{
		inLevel = false;
		HighestLevel = 1;
		
		LastLevel = SceneManager.sceneCountInBuildSettings-1;


		if (ls.DoesSaveFileExist())
		{
			int tmp = 1;

			ls.Load(out currentLevel, out tmp);
			HighestLevel = tmp;

		}

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

		inLevel = true;
		SceneManager.LoadScene(load,LoadSceneMode.Single);
		
		Time.timeScale = 1.0f;
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
		if(currentLevel<LastLevel)
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

	public bool PauseMenuActive()
	{
		if (PauseMenu != null)
		{
			return PauseMenu.activeSelf;
		}
		else
		{
			return false;
		}
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
		int highestLevel = HighestLevel;
		ls.Safe(currentLevel, highestLevel);
	}

}