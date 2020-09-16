using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<CharacterHealth>().TakeDamage(10f);
        }

        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}