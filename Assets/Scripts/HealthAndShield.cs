
using System;
using UnityEngine;

public class HealthAndShield : MonoBehaviour
{
    private static float MaxHealth { get;}
    private static float MaxShield { get;}

    private float currentHealth;
    private float currentShield;


    private void Awake()
    {
        currentHealth = MaxHealth;
        currentShield = MaxShield;
    }

    public void TakeDamage(float damage)
    {
        if (currentShield < damage && currentShield != 0)
        {
            damage -= currentShield;
            currentShield = 0;
        }
        
        else if (currentShield > damage)
        {
            currentHealth -= damage;
        }
        
        if(currentShield == 0)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
