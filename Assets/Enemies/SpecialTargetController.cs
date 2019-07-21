using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTargetController : MonoBehaviour, IRestartable
{
    public Health health;
    public int scoreAdded = 5;

    void Start()
    {
        health.OnDepletion += (e, args) => Die();
    }

    private void Die()
    {
        FindObjectOfType<PlayerController>()?.AddPoints(5);
        End();
    }

    public void End()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
    }

    public void SetPaused(bool paused)
    {
        GetComponent<Mover>().enabled = !paused;
    }
}
