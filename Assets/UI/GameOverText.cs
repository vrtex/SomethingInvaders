using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    public Text gameOverText;

    public void Setup(int score, int waves)
    {
        gameOverText.text = "Game over\n" + 
            "You got " + score + " points\n" +
            "You survived " + waves + " waves";
    }
    
}
