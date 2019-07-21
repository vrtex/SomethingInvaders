using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyInfo : MonoBehaviour
{
    public int diff = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetDiffuculty(int difficulty)
    {
        diff = difficulty;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("InvasionScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
