using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IRestartable
{
    [System.Serializable]
    public class MaterialInfo
    {
        public EnemyParser.EnemyType type;
        public Material material;
    }

    public WaveManager waveManager;

    public event System.EventHandler OnDestinationReached;

    private bool shouldMove = false;
    private Vector3 targetPosition;

    public Mover mover;

    public EnemyParser.EnemyType type;
    public Health health;

    public List<MaterialInfo> materials = new List<MaterialInfo>();
    public MeshRenderer meshRenderer;

    public Shooter weapon;
    private float reloadTime;
    private Coroutine shootCoroutine;

    public float healthChance = 0.3f;
    public float powerupChance = 0.05f;

    void Start()
    {
        health.OnDepletion += (s, e) => Die();
        weapon.team = Health.Team.Enemy;
        weapon.speed = -20;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!shouldMove)
            return;

        Vector3 difference = targetPosition - transform.position;

        float speedMultipler = difference.magnitude >= 1 ? 1 : difference.magnitude;

        if(difference.magnitude < 0.001f)
        {
            OnDestinationReached?.Invoke(this, new System.EventArgs());
            ForgetTarget();
        }
        else
        {
            difference.Normalize();
            mover.Move(difference.x * speedMultipler, difference.z * speedMultipler);
        }
    }

    public void Ready(bool value = true)
    {
        if(value)
        {
            shootCoroutine = StartCoroutine(ShootLoop());
            return;
        }

        if(shootCoroutine != null)
            return;

        StopCoroutine(shootCoroutine);
        shootCoroutine = null;
    }

    public void SetPaused(bool paused)
    {
        mover.enabled = !paused;
        weapon.enabled = !paused;
    }

    private IEnumerator ShootLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0.7f * reloadTime, 1.3f * reloadTime));

            weapon.Shoot();
        }
    }

    public void SetType(EnemyParser.EnemyType type)
    {
        this.type = type;
        EnemyParser.EnemyInfo info = EnemyParser.GetEnemyInfo(type);

        weapon.SetDamage(info.damage);
        health.SetMax(info.health);
        reloadTime = info.reloadTime / 1000;

        Material pickedMaterial = null;
        foreach(var material in materials)
        {
            if(material.type != type)
                continue;
            pickedMaterial = material.material;
            break;
        }
        if(pickedMaterial != null)
            meshRenderer.material = pickedMaterial;
    }

    public void Step(Vector2 step)
    {
        SetTargetPosition(transform.position + new Vector3(step.x, 0, step.y));
    }

    public void SetTargetPosition(Vector3 newTarget)
    {
        targetPosition = newTarget;
        shouldMove = true;
    }

    public void ForgetTarget()
    {
        targetPosition = transform.position;
        shouldMove = false;
    }

    private void Die()
    {
        ObjectPool.SpawnObject(ObjectPool.explosion, transform);
        float rand = Random.value;
        if(rand < healthChance)
        {
            ObjectPool.SpawnObject(ObjectPool.healthPickup, transform);
        }
        else if(rand < healthChance + powerupChance)
        {
            ObjectPool.SpawnObject(ObjectPool.powerUp, transform);
        }
        End();
    }

    public void Restart()
    {
        gameObject.SetActive(true);
    }

    public void End()
    {
        Ready(false);
        gameObject.SetActive(false);
    }
}
