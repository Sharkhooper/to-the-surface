using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPageController : MonoBehaviour {
	public int PageNr { get; set; }
	
	public void LoadLevel(int level)
	{
		level += (PageNr-1) * 6;
		Debug.Log("Load level " + level);
		GameManager.Instance.LoadLevel(level);
	}

	public void ShowNextPage()
	{
		transform.parent.parent.gameObject.GetComponent<LevelSelectController>().ShowNextPage(PageNr);
	}
	
	public void ShowPrevPage()
	{
		transform.parent.parent.gameObject.GetComponent<LevelSelectController>().ShowPrevPage(PageNr);
	}
}
