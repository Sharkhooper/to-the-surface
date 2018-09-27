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
		if(gm == null)
			gm = GameManager.Instance;
		int highestLevel = 5;
			//gm.HighestLevel;
		int lastLevel = gm.LastLevel;
		
		for (int i = 1; i <= 18; i++)
		{
			String buttonName = "ButtonLevel" + i;
			if (GameObject.Find(buttonName) != null)
			{
				GameObject button = GameObject.Find(buttonName);
				Image image = button.transform.Find("Image").GetComponent<Image>();
				// Level unlocked
				if (i <= highestLevel)
				{
					String loadStr = i < 10 ? "LevelSelect/Level0" + i : "LevelSelect/Level" + i;
					image.sprite = Resources.Load(loadStr, typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = true;
				}
				// Level locked
				else if (i <= lastLevel)
				{
					image.sprite = Resources.Load("LevelSelect/lockedTEMP", typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = false;
				}
				// Level doesn't exist
				else
				{
					button.SetActive(false);
					
					// Deactivates NextPageButton because there isnt' a next page
					GameObject nextPage = button.transform.parent.Find("ButtonNextPage").gameObject;
					nextPage.SetActive(false);
					
					// Next page can't be opened so buttons on it don't have to be changed
					if (i % 6 == 0)
						return;
				}
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

	public void LoadLevel(int level)
	{
		gm.LoadLevel(level);
	}

	public void OpenLevelSelect()
	{
		ActivateAllPages();
		UpdateLevelSelect();
		DisablePagesAfterFirst();
	}
}
