using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int playerScore = 0;

    public TMP_Text playerScoreText;

    public int CurrentScore
    {
        get { return playerScore; }

    }

    public void PlayerGoal()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
    }

}
