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
            other.gameObject.GetComponent<Swarmer>().ExplosionForce(other.contacts[0].point);
        }

        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}