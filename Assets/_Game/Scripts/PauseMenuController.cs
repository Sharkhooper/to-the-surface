using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenuController : MonoBehaviour
{
	private GameManager gm;

	private void Start()
	{
		gm = GameManager.Instance;
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
