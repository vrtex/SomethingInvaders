using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IRestartable
{
    public ProjectileCollider projectileCollider;

    private int damage;
    private Health.Team team;
    public MeshRenderer projectileRenderer;
    public List<Material> materials;

    private void Start()
    {
        projectileCollider.OnTrigger += CollisionDetected;
    }

    private void CollisionDetected(object sender, ProjectileCollider.ProjectileCollisionArgs e)
    {
        Health health = e.other.transform.root.GetComponent<Health>();
        if(health == null)
            return;

        if(health.team == team)
            return;


        health.Damage(damage);
        End();
    }

    public void Setup(Health.Team team, int damage, float speed)
    {
        this.damage = damage;
        this.team = team;
        projectileRenderer.material = materials[Mathf.Clamp(damage - 1, 0, 2)];
        GetComponent<Mover>().speed = new Vector2(0, speed);
    }

    public void SetPaused(bool paused)
    {
        GetComponent<Mover>().enabled = !paused;
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
