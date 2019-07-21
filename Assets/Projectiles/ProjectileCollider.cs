using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    public class ProjectileCollisionArgs : EventArgs
    {
        public Collider other;
    }

    public event EventHandler<ProjectileCollisionArgs> OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(this, new ProjectileCollisionArgs { other = other });
    }
}
