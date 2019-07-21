using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKiller : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        IRestartable Restarter = other.transform.root.GetComponent<IRestartable>();
        if(Restarter == null)
            return;
        Restarter.End();
    }
}
