using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Rootin
// Dootin
// Shootin

public class Shooter : MonoBehaviour
{
    public event System.EventHandler OnDamageChange;

    public AudioSource shootSound;

    public Health.Team team;
    public int damage { get; private set; }
    public float speed = 20;

    public void Shoot()
    {
        if(!enabled)
            return;
        ProjectileController projectile = ObjectPool.SpawnObject("Bolt", transform).GetComponent<ProjectileController>();
        projectile.Setup(team, damage, speed);

        if(shootSound != null)
            shootSound.Play();
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void ChangeDamage(int change)
    {
        damage = Mathf.Clamp(damage + change, 1, 3);
        OnDamageChange?.Invoke(this, null);
    }
}
