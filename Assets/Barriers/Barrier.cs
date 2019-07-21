using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        health.OnDepletion += (e, args) => Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
