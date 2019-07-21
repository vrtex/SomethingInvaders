using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IRestartable
{
    public ParticleSystem particles;

    public void End()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        particles.Play();
    }

    private void Update()
    {
        if(!particles.IsAlive())
            End();
    }
}
