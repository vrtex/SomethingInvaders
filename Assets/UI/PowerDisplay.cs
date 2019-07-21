using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    public Shooter weapon;
    public Slider slider;

    private void Awake()
    {
        weapon.OnDamageChange += Weapon_OnDamageChange;
        Weapon_OnDamageChange(null, null);
    }

    private void Weapon_OnDamageChange(object sender, System.EventArgs e)
    {
        slider.value = weapon.damage / 3.0f;
    }
}
