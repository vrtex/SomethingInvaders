using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Special")
            return;

        other.transform.root.gameObject.SetActive(false);
    }
}
