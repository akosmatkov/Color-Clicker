using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject records;
    [SerializeField] GameObject startScreenButtons;
    [SerializeField] Text recordText;

    private bool showRecords = false;

    public void CallStartGame()
    {
        SceneController.instance.StartGame();
    }

    public void CallQuitGame()
    {
        SceneController.instance.QuitGame();
    }

    public void CallMainMenuLoad()
    {
        SceneController.instance.LoadMainMenu();
    }

    public void ShowRecords()
    {
        startScreenButtons.SetActive(showRecords);
        records.SetActive(!showRecords);

        recordText.text = GameManager.LastRecord.ToString();

        showRecords = !showRecords;
    }
}
