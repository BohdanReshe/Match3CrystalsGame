using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject restartButton;
    public GameObject timerTextObject;
    [SerializeField] TextMeshProUGUI scoresText;
    [SerializeField] TextMeshProUGUI movesText;
    [SerializeField] TextMeshProUGUI timerText;
    private int currentScores = 0;
    [SerializeField] int currentMoves = 30;
    [SerializeField] int currentTimer = 30;

    void Start()
    {
        instance = GetComponent<GameManager>();
        InvokeRepeating("TimerCount", 0, 1);
        scoresText.text = "Scores: " + currentScores;
        movesText.text = "Moves: " + currentMoves;
    }

    public void ScoreUpdate(int numberToUpdate)
    {
        currentScores += numberToUpdate;
        scoresText.text = "Scores: " + currentScores;
    }
    public void MovesUpdate(int numberToUpdate)
    {
        if (currentMoves == 0)
        {
            GameOver();
        }
        else
        {
            currentMoves += numberToUpdate;
            movesText.text = "Moves: " + currentMoves;
        }
    }

    void TimerCount()
    {
        if (currentTimer <= 0)
        {
            GameOver();
        }
        else
        {
            currentTimer -= 1;
            timerText.text = "Time: " + currentTimer;
        }

    }
    void GameOver()
    {
        restartButton.SetActive(true);
        timerTextObject.SetActive(false);
        ItemController.isGameOver = true;
    }
    public void RestartButtonPressed()
    {
        BoardManager.instance.ShuffleTheBoard();
        timerTextObject.SetActive(true);
        restartButton.SetActive(false);
        currentTimer = 30;
        timerText.text = "Time: " + currentTimer;
        currentMoves = 30;
        movesText.text = "Moves: " + currentMoves;
        ItemController.isGameOver = false;
    }
}