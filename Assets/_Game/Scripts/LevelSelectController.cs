using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour {
	[SerializeField] private GameObject pagePrefab;
	[SerializeField] private int nonPageObjects = 3;
	
	private void Start() {
		InitalizeLevelSelect();
	}

	/// <summary>
	/// Initializes LevelSelect
	/// </summary>
	public void InitalizeLevelSelect()
	{
		int highestLevel = GameManager.Instance.HighestLevel;
		int lastLevel = GameManager.Instance.LastLevel;
		int pagesNeeded = lastLevel % 6 == 0? lastLevel/6 : lastLevel/6 + 1;
		int buttonsOnLastPage = lastLevel % 6;
		GameObject currentPage;

		for (int i = 1; i <= pagesNeeded; i++)
		{
			currentPage = GameObject.Instantiate(pagePrefab, transform.GetChild(0));
			currentPage.GetComponent<LevelSelectPageController>().PageNr = i;
			
			//Load images for buttons
			for (int j = 1; j <= 6; j++)
			{
				String buttonName = "ButtonLevel" + j;
				int levelNr = j + (currentPage.GetComponent<LevelSelectPageController>().PageNr-1) * 6;

				GameObject button = currentPage.transform.Find(buttonName).gameObject;
				Image image = button.transform.Find("Image").GetComponent<Image>();
				// Level unlocked
				if (levelNr <= highestLevel)
				{
					string loadStr = levelNr < 10 ? "LevelSelect/Level0" + levelNr : "LevelSelect/Level" + levelNr;
					image.sprite = Resources.Load(loadStr, typeof(Sprite)) as Sprite;
					button.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = levelNr + "";
					button.GetComponent<Button>().interactable = true;
				}
				// Level locked
				else if (levelNr <= lastLevel)
				{
					image.sprite = Resources.Load("LevelSelect/lockedTEMP", typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = false;
				}
			}
			
			// Deactivate not needed NextPage- & PreviousPage-Buttons
			if (i == 1)
			{
				currentPage.transform.Find("ButtonPrevPage").gameObject.SetActive(false);
			}
			if (i == pagesNeeded)
			{
				currentPage.transform.Find("ButtonNextPage").gameObject.SetActive(false);
				
				// Deactivate not needed level buttons
				if (buttonsOnLastPage != 0)
				{
					for (int j = 6; j > buttonsOnLastPage; j--)
					{
						String buttonName = "ButtonLevel" + j;
						currentPage.transform.Find(buttonName).gameObject.SetActive(false);
					}
				}
			}
		}
		DisablePagesAfterFirst();
	}

	/// <summary>
	/// Disables all pages except the first one
	/// </summary>
	private void DisablePagesAfterFirst() {
		bool firstPage = true;
		foreach (GameObject page in GameObject.FindGameObjectsWithTag("LevelSelectPage")) {
			if (firstPage) 
			{
				firstPage = false;
			} 
			else 
			{
				page.SetActive(false);
			}
		}
	}

	public void ShowNextPage(int pageNr)
	{
		GameObject canvas = transform.Find("Canvas").gameObject;
		canvas.transform.GetChild(pageNr + nonPageObjects - 1).gameObject.SetActive(false);
		canvas.transform.GetChild(pageNr + nonPageObjects).gameObject.SetActive(true);
	}
	
	public void ShowPrevPage(int pageNr)
	{
		GameObject canvas = transform.Find("Canvas").gameObject;
		canvas.transform.GetChild(pageNr + nonPageObjects - 1).gameObject.SetActive(false);
		canvas.transform.GetChild(pageNr + nonPageObjects - 2).gameObject.SetActive(true);
	}
}
