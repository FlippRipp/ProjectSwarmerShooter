using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;

    private float damage = 10;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
        }

        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}