using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player plr;
    [SerializeField] TextMeshProUGUI maxAmmo;
    [SerializeField] TextMeshProUGUI currentAmmo;
    [SerializeField] TextMeshProUGUI currentComboLevel;
    [SerializeField] TextMeshProUGUI currentScoreTxt;

    [SerializeField] TextMeshProUGUI highScoreLose;
    [SerializeField] TextMeshProUGUI highScorePause;
    
    [SerializeField] TextMeshProUGUI currentScoreLose;

    [SerializeField] Transform ComboCounter;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject EndGameScreen;
    [SerializeField] GameObject WinGameWindow;
    [SerializeField] GameObject EndGameWindow;

    [SerializeField] int maxComboLevel;
    [SerializeField] int comboRequired;

    public static int comboCount = 0;
    public static int comboLevel = 1;

    bool isPauseActive = false;


    private void Start()
    {
        Cursor.visible = false;
        maxAmmo.text = plr.maxBullets.ToString();
        StartCoroutine(loseComboCount());
        StartCoroutine(loseScore());
    }

    private void Update()
    {
        currentAmmo.text = plr.currentBullets.ToString();
     

        if(GameManager.currentScore < 0)
        {
            GameManager.currentScore = 0;
        }

        if(comboCount >= comboRequired && comboLevel == maxComboLevel)
        {
            comboCount = comboRequired;
        }
        else if(comboCount >= comboRequired)
        {
            comboLevel += 1;
            comboCount -= comboRequired;
        }
        else if(comboCount < 0 && comboLevel > 1)
        {
            comboLevel--;
            comboCount = comboRequired - 1;
        }
        else if(comboCount < 0 && comboLevel == 1)
        {
            comboCount = 0;
        }
        ComboCounter.localScale = new Vector3((float)comboCount / (float)comboRequired, 1, 1);
        currentComboLevel.text = "x" + comboLevel.ToString();
        currentScoreTxt.text = GameManager.currentScore.ToString();
    }

    IEnumerator loseComboCount()
    {
        yield return new WaitForSeconds(0.1f);
        comboCount-= 10;
        StartCoroutine(loseComboCount());
    }

    IEnumerator loseScore()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.currentScore--;
        StartCoroutine(loseScore());
    }

    public void InstantiateLoseWindow()
    {
        Cursor.visible = true;
        EndGameWindow.SetActive(true);
        EndGameScreen.SetActive(true);
        currentScoreLose.text = GameManager.currentScore.ToString();
        highScoreLose.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void InstantiateWinWindow()
    {
        Cursor.visible = true;
        WinGameWindow.SetActive(true);
    }

    public void PauseGameButtonClicked()
    {
        if(PauseMenu != null)
        {
            if (Time.timeScale == 1)
            {
                PauseMenu.SetActive(true);
                Cursor.visible = true;
                isPauseActive = true;
                highScorePause.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
                Time.timeScale = 0;
            }
            else if (isPauseActive)
            {
                PauseMenu.SetActive(false);
                Cursor.visible = false;
                isPauseActive = false;
                Time.timeScale = 1;
            }
        }
    }

    public static void IncreaseComboCount()
    {
        comboCount+=200;
        GameManager.currentScore += 100 * comboLevel;
    }

}
