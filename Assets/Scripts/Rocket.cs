using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;

    private float rocketLiftOffThrust;
    private float rocketSpeed;
    private float rocketLiftOffTime;
    private float rocketDamage;
    private Vector3 rocketTargetPos;
    private Vector3 rocketTargetDir;
    private Transform rocketTarget;
    private Transform rocketActivator;

    private Rigidbody body;

    private float timeOnSpawn;
    private bool lockedOnTarget;
    

    private void OnEnable()
    {
        timeOnSpawn = Time.time;
        body = GetComponent<Rigidbody>();
    }


    public void Init(float liftoffTime, float damage, float speed, float liftOffThrust, Transform target, Transform activator, Vector3 targetPos)
    {
        rocketTargetDir = Vector3.zero;
        rocketLiftOffTime = liftoffTime;
        rocketSpeed = speed;
        rocketLiftOffThrust = liftOffThrust;
        rocketDamage = damage;
        rocketActivator = activator;
        rocketTarget = target;
        if (rocketTarget != activator)
        {
            if (rocketTarget.CompareTag("Player") || target.CompareTag("Enemy"))
            {
                lockedOnTarget = true;
                return;
            }
        }

        rocketTargetPos = targetPos;
        lockedOnTarget = false;
    }

    private void FixedUpdate()
    {
        if (Time.time - timeOnSpawn < rocketLiftOffTime)
        {
            transform.LookAt(Vector3.up + transform.position);
            body.AddForce(Vector3.up * rocketLiftOffThrust);
        }
        else
        {
            body.useGravity = false;
            if (lockedOnTarget)
            {
                transform.LookAt(rocketTarget);
            }
            else
            {
                if (rocketTargetDir == Vector3.zero)
                {
                    rocketTargetDir = (rocketTargetPos - transform.position).normalized;
                } 
                transform.LookAt(transform.position + rocketTargetDir);
            }
            body.velocity = transform.forward * rocketSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")) return;
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
