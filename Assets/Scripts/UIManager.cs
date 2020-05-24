using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text timerText;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Text loseMenuScoreText;

    GameSession gameSession;
    bool showPauseMenu = false;

    public event Action<bool> OnPauseGame;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.OnScoreChanged += SetScoreText;
        gameSession.OnUpdateTimer += SetTimerText;
        gameSession.OnGameOver += ShowLoseMenu;
    }

    private void SetTimerText(float timerValue)
    {
        timerText.text = timerValue.ToString();
    }

    private void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = showPauseMenu == true? 1 : 0;
        pauseMenu.SetActive(!showPauseMenu);
        OnPauseGame?.Invoke(showPauseMenu);
        showPauseMenu = !showPauseMenu;
    }

    private void ShowLoseMenu(int score)
    {
        loseMenu.SetActive(true);
        loseMenuScoreText.text = score.ToString();
    }

    public void CallRestartLevel()
    {
        SceneController.instance.RestartLevel();
    }

    public void CallMainMenuLoad()
    {
        SceneController.instance.LoadMainMenu();
    }
}
