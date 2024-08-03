using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int currentScore = 0;

    public UIManager uiManag;
    public bool LevelLoaded = false;
    public bool hasGameStarted = false;

    public static void CheckIfScoreBeaten()
    {
        if(currentScore > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }

    private void Awake()
    {
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(LevelLoaded && FindObjectsOfType<AbstractEnemy>().Length == 0)
        {
            WinGame();
        }
    }

    public void KillPlayer()
    {
        Time.timeScale = 0;
        uiManag.InstantiateLoseWindow();
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        uiManag.InstantiateWinWindow();
    }

    public void LoadNextFloor()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
