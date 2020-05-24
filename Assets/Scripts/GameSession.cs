using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    #region References

    // Ссылки на Image Component кнопок для изменения их цвета
    [SerializeField] Image[] buttonImages;

    // Ссылка на текст с названием заданного цвета
    [SerializeField] Text colorNameText;
    [SerializeField] float timerCountdown;
    [SerializeField] GameObject mainGameCanvas;
    [SerializeField] List<Color> colorsToAttach;

    #endregion

    #region Initialization

    private int score = 0;

    // Массив названий цветов для случайного назначения
    string[] colorNames = new string[] { "Красный", "Оранжевый", "Жёлтый", "Зелёный", "Голубой", "Синий", "Фиолетовый" };

    // Список цветов для случайного назначения
    //List<Color> colors = new List<Color> { Color.blue, Color.red, Color.green, Color.yellow, Color.black, Color.white };

    #endregion

    public event Action<int> OnScoreChanged;
    public event Action<float> OnUpdateTimer;
    public event Action<int> OnGameOver;

    private void Awake()
    {
        FindObjectOfType<UIManager>().OnPauseGame += ShowMainGameCanvas;
        var buttonTriggers = FindObjectsOfType<ButtonTrigger>();

        foreach (ButtonTrigger b in buttonTriggers)
        {
            b.OnColorChoose += CheckColorMatch;
        }

        SetTextAndButtonsColors();

        Time.timeScale = 1;
    }

    void Start()
    {
        OnScoreChanged?.Invoke(score);
    }

    private IEnumerator UpdateTimer()
    {
        float timeToWait = 1f;
        float tempCountdown = timerCountdown;

        OnUpdateTimer?.Invoke(tempCountdown);

        while(tempCountdown > 0)
        {
            yield return new WaitForSeconds(timeToWait);
            OnUpdateTimer?.Invoke(tempCountdown - timeToWait);
            tempCountdown -= timeToWait;
        }

        ShowMainGameCanvas(false);
        OnGameOver?.Invoke(score);
    }

    public void SetTextAndButtonsColors()
    {
        var nameIndex = UnityEngine.Random.Range(0, colorNames.Length); // Индекс случайного названия цвета в массиве
        var colorIndex = UnityEngine.Random.Range(0, colorsToAttach.Count); // Индекс случайного цвета в списке

        colorNameText.text = colorNames[nameIndex];
        colorNameText.color = colorsToAttach[colorIndex];

        SetButtonColors(colorNameText.color);

        StartCoroutine(UpdateTimer());
    }

    private void SetButtonColors(Color color)
    {
        var tempColors = new List<Color>();  // Временный список цветов
        tempColors.AddRange(colorsToAttach);
        var randomButtonIndex = UnityEngine.Random.Range(0, buttonImages.Length); // Индекс случайной кнопки, которой назначено название правильного цвета

        buttonImages[randomButtonIndex].color = color;
        tempColors.Remove(color); // Правильный цвет удаляется из временного списка цветов для избежания повторов

        foreach (Image button in buttonImages)
        {
            var randomColorIndex = UnityEngine.Random.Range(0, tempColors.Count); // Индекс случайного цвета

            // Если текущая кнопка та, которой уже назначен правильный цвет, начинается новая итерация цикла
            if (button == buttonImages[randomButtonIndex]) 
            {
                continue;
            }

            // Если нет, кнопке назначается случайный цвет
            button.color = tempColors[randomColorIndex];
            // Этот цвет удаляется из временного списка цветов для избежания повторов
            tempColors.Remove(tempColors[randomColorIndex]);
        }

        tempColors.Clear();
    }

    private void CheckColorMatch(Color color)
    {
        if(colorNameText.color == color)
        {
            UpdateScore();

            if (score > GameManager.LastRecord)
            {
                GameManager.LastRecord = score;
            }

            StopAllCoroutines();
            SetTextAndButtonsColors();
        }
        else
        {
            ShowMainGameCanvas(false);
            OnGameOver?.Invoke(score);
        }
    }

    private void ShowMainGameCanvas(bool showCanvas)
    {
        mainGameCanvas.SetActive(showCanvas);
    }

    private void UpdateScore()
    {
        score++;
        OnScoreChanged.Invoke(score);
    }
}
