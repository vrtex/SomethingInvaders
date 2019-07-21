using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IRestartable
{
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject.tag != "Player")
            return;

        Health health = other.transform.root.GetComponent<Health>();
        health.Restore(1);
        End();
    }

    public void Restart()
    {
        gameObject.SetActive(true);
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
