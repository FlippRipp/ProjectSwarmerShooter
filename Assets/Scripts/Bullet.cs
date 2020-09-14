
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        
            Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
    }
}
