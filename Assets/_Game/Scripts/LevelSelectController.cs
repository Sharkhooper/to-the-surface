using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
	private GameManager gm;
	
	// Use this for initialization
	void Start ()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		ActivateAllPages();
		UpdateLevelSelect();
		DisablePagesAfterFirst();
	}

	// Locks all currently not unlocked levels
	public void UpdateLevelSelect()
	{
		int highestLevel = gm.HighestLevel;
		
		//Static implementation for last level
		for (int i = highestLevel+1; i <= 18; i++)
		{
			String buttonName = "ButtonLevel" + i;
			if (GameObject.Find(buttonName) != null)
			{
				GameObject button = GameObject.Find(buttonName);
				Image image = button.transform.Find("Image").GetComponent<Image>();
				image.sprite = Resources.Load("lockedTEMP", typeof(Sprite)) as Sprite;
				button.GetComponent<Button>().interactable = false;
			}
			else
			{
				Debug.Log("Button not found");
			}
		}
	}

	//Disables all pages except the first one
	private void DisablePagesAfterFirst()
	{
		bool firstPage = true;
		foreach(GameObject page in GameObject.FindGameObjectsWithTag("LevelSelectPage"))
		{
			if(firstPage)
				firstPage = false;
			else
				page.SetActive(false);
		}
	}

	//Ensures all pages are active before loading "locked" images
	private void ActivateAllPages()
	{
		foreach(GameObject page in GameObject.FindGameObjectsWithTag("LevelSelectPage"))
		{
			page.SetActive(true);
		}
	}
}
