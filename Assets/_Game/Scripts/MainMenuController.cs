using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameManager gm;
    
    private void Awake()
    {
        if (gm == null)
        {
            gm = GameManager.Instance;
        }
        gm.inLevel = false;
    }

    public void Play()
    {
        gm.ContinueGame();
    }

    public void LoadLevel(int level)
    {
        gm.LoadLevel(level);
    }

    public void Exit()
    {
        Application.Quit();
    }
    
   
}
