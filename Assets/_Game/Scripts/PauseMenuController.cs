using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenuController : MonoBehaviour
{

	// Use this for initialization




	private GameManager gm;


	private void Start()
	{

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

	}



	public void GoMainMenu()
	{
		gm.GoMainMenu();
		
	}

	public void ContinueLevel()
	{
		gm.ContinueLevel();
		
	}

	public void ResetLevel()
	{
		gm.ResetLevel();
	}



}
