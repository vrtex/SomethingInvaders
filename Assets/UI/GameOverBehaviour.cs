using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBehaviour : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync("InvasionScene");
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
