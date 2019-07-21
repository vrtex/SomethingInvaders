using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    public event System.EventHandler OnEnemyDetected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Enemy")
            return;

        OnEnemyDetected?.Invoke(this, null);
    }
}
