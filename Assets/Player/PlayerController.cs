using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event System.EventHandler OnScoreChange;

    public Mover mover;
    public Vector2 horizontalBounds;
    public Vector2 verticalBounds;

    public Shooter weapon;
    public int defaultDamage = 1;
    public float reloadTime = 0.3f;
    private float lastShot = 0;
    private bool canShoot = false;

    public int currentScore { get; private set; } = 0;

    void Start()
    {

        weapon.team = Health.Team.Player;
        weapon.SetDamage(defaultDamage);

        lastShot = Time.time - reloadTime;

        mover.horizontalBounds = new System.Tuple<float, float>(horizontalBounds.x, horizontalBounds.y);
        mover.verticalBounds = new System.Tuple<float, float>(verticalBounds.x, verticalBounds.y);
    }

    void Update()
    {
        mover.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        TryShoot();

        if(Input.GetKeyDown(KeyCode.E))
        {
            ObjectPool.SpawnObject(ObjectPool.explosion, transform);
        }
    }

    public void SetPaused(bool paused)
    {
        mover.enabled = !paused;
        weapon.enabled = !paused;
    }

    public void AddPoints(int amount)
    {
        currentScore += amount;
        OnScoreChange?.Invoke(this, null);
    }

    public void SetCanShoot(bool value)
    {
        canShoot = value;
        weapon.ChangeDamage(0);
    }

    private void TryShoot()
    {
        if(lastShot + reloadTime > Time.time || !canShoot)
            return;

        if(Input.GetAxis("Shoot") < 0.9f)
            return;

        weapon.Shoot();
        lastShot = Time.time;
    }
}
