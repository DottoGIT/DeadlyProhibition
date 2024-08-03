using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        Cursor.visible = true;
        GameManager.CheckIfScoreBeaten();
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        GameManager.currentScore = 0;
        UIManager.comboCount = 0;
        UIManager.comboLevel = 1;
        SceneManager.LoadScene(1);
    }
}
