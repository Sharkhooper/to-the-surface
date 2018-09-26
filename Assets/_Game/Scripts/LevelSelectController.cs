using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class LevelSelectController : MonoBehaviour
{
	private GameManager gm;
	
	// Use this for initialization
	void Start ()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		UpdateLevelSelect();
	}

	void UpdateLevelSelect()
	{
		Debug.Log("Updatet LevelSelect");
		int highestLevel = gm.HighestLevel;
		//Static implementation for last level
		for (int i = highestLevel+1; i < 18; i++)
		{
			String buttonName = "ButtonLevel" + i;
			GameObject button = GameObject.Find(buttonName);
		}
	}
}
