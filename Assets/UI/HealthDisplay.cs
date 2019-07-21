using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public GameObject heartClass;
    public Health playerHealth;
    public GridLayoutGroup grid;
    public float disabledHeartAlpha = 0.2f;

    private void Start()
    {
        for(int i = 0; i < playerHealth.maxHealth; ++i)
        {
            Instantiate(heartClass, transform);
        }

        playerHealth.OnChange += PlayerHealth_OnChange;
        PlayerHealth_OnChange(null, null);
    }

    private void Update()
    {
        PlayerHealth_OnChange(null, null);
    }

    private void PlayerHealth_OnChange(object sender, Health.HealthEventArgs e)
    {
        for(int i = 0; i < playerHealth.maxHealth; ++i)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, i < playerHealth.currentHealth ? 1 : disabledHeartAlpha);
            //transform.GetChild(i).gameObject.SetActive(i < playerHealth.currentHealth);
        }
    }

}
