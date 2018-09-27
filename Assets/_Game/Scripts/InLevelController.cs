using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelController : MonoBehaviour {

	private GameManager gm;
    
	private void Awake()
	{
		gm = GameManager.Instance;
		gm.inLevel = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!gm.PauseMenuActive())
			{
				gm.PauseLevel();
			}
			else
			{
				gm.ContinueLevel();
			}
		}
	}

	public void LevelFinished()
	{
		gm.LevelFinished();
	}
}
