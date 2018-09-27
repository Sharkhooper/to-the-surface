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
		gm = GameManager.Instance;
		ActivateAllPages();
		UpdateLevelSelect();
		DisablePagesAfterFirst();
	}

	// Locks all currently not unlocked levels
	public void UpdateLevelSelect()
	{
		int highestLevel = gm.HighestLevel;
		int lastLevel = 10;
			//gm.LastLevel;
		int pageSize = 6;
		
		for (int i = 1; i <= 18; i++)
		{
			String buttonName = "ButtonLevel" + i;
			if (GameObject.Find(buttonName) != null)
			{
				GameObject button = GameObject.Find(buttonName);
				Image image = button.transform.Find("Image").GetComponent<Image>();
				if (i <= highestLevel)
				{
					String loadStr = i < 10 ? "LevelSelect/Level0" + i : "LevelSelect/Level" + i;
					image.sprite = Resources.Load(loadStr, typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = true;
				}
				else if (i <= lastLevel)
				{
					image.sprite = Resources.Load("LevelSelect/lockedTEMP", typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = false;
				}
				else
				{
					Debug.Log("too many buttons");
					button.SetActive(false);
					if (i % 6 == 0)
						DisableFollowingPages(i/6+1);
						return;
				}
			}
			else
			{
				Debug.Log("Button not found");
			}
		}
	}

	private void DisableFollowingPages(int page)
	{
		if(page == 1)
			if(GameObject.Find("ButtonNextPage1") != null)
				GameObject.Find("ButtonNextPage1").SetActive(false);
			else
			{
				Debug.Log("not found 1");
			}
		if(page==2)
			if(GameObject.Find("ButtonNextPage2") != null)
				GameObject.Find("ButtonNextPage2").SetActive(false);
			else
			{
				Debug.Log("not found 2");
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
