using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpawner : MonoBehaviour
{
    public float spawnTimer = 10;
    public float randomAddition = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTimer + Random.Range(spawnTimer - randomAddition, spawnTimer + randomAddition));
            var target = ObjectPool.SpawnObject(ObjectPool.special, transform);
            target.SetActive(true);
        }
    }

}
