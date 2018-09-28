using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour {
	private void Start() {
		ActivateAllPages();
		UpdateLevelSelect();
		DisablePagesAfterFirst();
	}

	/// <summary>
	/// Locks all currently not unlocked levels
	/// </summary>
	public void UpdateLevelSelect() {
		int highestLevel = GameManager.Instance.HighestLevel;
		int lastLevel = GameManager.Instance.LastLevel;

		for (int i = 1; i <= 18; i++) {
			string buttonName = "ButtonLevel" + i;
			if (GameObject.Find(buttonName) != null) {
				GameObject button = GameObject.Find(buttonName);
				Image image = button.transform.Find("Image").GetComponent<Image>();
				// Level unlocked
				if (i <= highestLevel) {
					string loadStr = i < 10 ? "LevelSelect/Level0" + i : "LevelSelect/Level" + i;
					image.sprite = Resources.Load(loadStr, typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = true;
				}
				// Level locked
				else if (i <= lastLevel) {
					image.sprite = Resources.Load("LevelSelect/lockedTEMP", typeof(Sprite)) as Sprite;
					button.GetComponent<Button>().interactable = false;
				}
				// Level doesn't exist
				else {
					button.SetActive(false);

					// Deactivates NextPageButton because there isnt' a next page
					if (button.transform.parent.Find("ButtonNextPage") != null)
					{

						GameObject nextPage = button.transform.parent.Find("ButtonNextPage").gameObject;
						
						nextPage.SetActive(false);
					}

					// Next page can't be opened so buttons on it don't have to be changed
					if (i % 6 == 0)
						return;
				}
			}
		}
	}

	/// <summary>
	/// Disables all pages except the first one
	/// </summary>
	private void DisablePagesAfterFirst() {
		bool firstPage = true;
		foreach (GameObject page in GameObject.FindGameObjectsWithTag("LevelSelectPage")) {
			if (firstPage) {
				firstPage = false;
			} else {
				page.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Ensures all pages are active before loading "locked" images
	/// </summary>
	private void ActivateAllPages() {
		foreach (GameObject page in GameObject.FindGameObjectsWithTag("LevelSelectPage")) {
			page.SetActive(true);
		}
	}

	public void LoadLevel(int level) {
		GameManager.Instance.LoadLevel(level);
	}

	public void OpenLevelSelect() {
		ActivateAllPages();
		UpdateLevelSelect();
		DisablePagesAfterFirst();
	}
}
