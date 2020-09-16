using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private bool regen;
    [SerializeField] private float regenCoolDown = 5;
    [SerializeField] private float regenPerSecond = 4;

    private float currentHealth;
    private float lastDamageTime;
    public UnityEvent CharacterDied;
    public UnityEvent<float> CharacterHealthChanged;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Regen();
    }

    private void Regen()
    {
        if (regen && Time.time - lastDamageTime > regenCoolDown)
        {
            if (currentHealth < maxHealth)
            {
                CharacterHealthChanged?.Invoke(currentHealth);
                currentHealth += regenPerSecond * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(currentHealth);
        lastDamageTime = Time.time;
        currentHealth -= damage;
        CharacterHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            CharacterDied?.Invoke();
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}