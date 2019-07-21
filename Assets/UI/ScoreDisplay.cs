using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public PlayerController player;
    public Text scoreText;

    private void Start()
    {
        UpdateScore();
        player.OnScoreChange += (e, args) => UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: \n" + player.currentScore;
    }
}
