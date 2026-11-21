using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PongManager : MonoBehaviour
{

    public ScoreManager scoreManager;
    public GameObject gameOverPanel;
    public GameObject ball;
    public GameObject middleBar;

    public TMP_Text finalScoreText;
    public TMP_Text recordText;
    public TMP_Text moneyText;
    public TMP_Text expText;

    public int moneyPerPoint = 1;
    public int expPerPoint = 2;

    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        gameOverPanel.SetActive(false);
    }

    public void EndGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        int score = scoreManager.CurrentScore;

        int moneyEarned = score * moneyPerPoint;
        int expEarned = score * expPerPoint;

        PlayerData.UpdatePongRecord(score);
        PlayerData.AddMoney(moneyEarned);
        PlayerData.AddExperience(expEarned);

        finalScoreText.text = "Pontuação: " + score.ToString();
        recordText.text = "Recorde: " + PlayerData.PongRecord.ToString();
        moneyText.text = "Dinheiro: +" + moneyEarned;
        expText.text = "+" + expEarned + " XP";

        gameOverPanel.SetActive(true);
        ball.SetActive(false);
        middleBar.SetActive(false);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}