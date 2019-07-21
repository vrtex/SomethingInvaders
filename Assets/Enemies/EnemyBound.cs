using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBound : MonoBehaviour
{
    public WaveManager waveManager;
    private int inside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.tag != "Enemy")
            return;

        ++inside;

        if(inside == 1)
            waveManager.GetComponent<WaveManager>().Reverse();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.gameObject.tag != "Enemy")
            return;

        --inside;
    }
}
