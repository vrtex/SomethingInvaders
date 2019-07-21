using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public CanvasGroup pauseUI;
    public WaveManager waveManager;

    private bool paused = false;
    private bool keyDown;

    private void Start()
    {
        keyDown = Input.GetAxis("Pause") > 0.9f;
    }

    private void Update()
    {
        if(keyDown)
        {
            keyDown = Input.GetAxis("Pause") > 0.9f;
            return;
        }
        else if(Input.GetAxis("Pause") > 0.9f)
        {
            keyDown = true;
            TogglePause();
        }
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

    public void Resume()
    {

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
