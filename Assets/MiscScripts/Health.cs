using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Serializable]
    public enum Team
    {
        Player,
        Enemy,
        Unknown
    }

    public class HealthEventArgs : EventArgs
    {
        public int previous;
        public int current;
        public int change;
    }

    public event EventHandler<HealthEventArgs> OnChange;
    public event EventHandler<HealthEventArgs> OnDepletion;

    public int currentHealth = 1;
    public int maxHealth = 1;
    public Team team;

    // Start is called before the first frame update
    void Start()
    {
        if(maxHealth < currentHealth)
        {
            Debug.LogError(gameObject.name + " has a messed up health. Fix this boo-boo");
        }
    }

    public void SetMax(int max)
    {
        maxHealth = currentHealth = max;
    }

    public void Damage(int amount)
    {
        int previous = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnChange?.Invoke(this, 
            new HealthEventArgs { previous = previous, current = currentHealth, change = currentHealth - previous });

        if(currentHealth == 0)
            OnDepletion?.Invoke(this,
                new HealthEventArgs { previous = previous, current = currentHealth, change = currentHealth - previous });
    }

    public void Restore(int amount)
    {
        int previous = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnChange?.Invoke(this,
            new HealthEventArgs { previous = previous, current = currentHealth, change = currentHealth - previous });

    }
}
