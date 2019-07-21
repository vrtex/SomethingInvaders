using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public CanvasGroup pauseUI;
    public WaveManager waveManager;

    private bool paused = false;


    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;

        var projectiles = FindObjectsOfType<ProjectileController>();
        foreach(var p in projectiles)
            p.SetPaused(paused);

        waveManager.SetPaused(paused);

        var movers = FindObjectsOfType<Mover>();
        foreach(var m in movers)
            m.enabled = !paused;

        var shooters = FindObjectsOfType<Shooter>();
        foreach(var s in shooters)
            s.enabled = !paused;

        pauseUI.alpha = paused ? 1 : 0;
        pauseUI.interactable = paused;
        pauseUI.blocksRaycasts = paused;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
