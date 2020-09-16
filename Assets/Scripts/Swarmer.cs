using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swarmer : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField] private Transform target;
    [SerializeField] private EnemySwarmData enemySwarmData;
    private float waverAmount = 10;

    private float randomMaxSpeed;
    private float rotationSpeed;
    private float currentSpeed;
    private float reflex;
    private Vector3 directionToTarget;
    private float timeBetweenJumps;
    private bool isFleeing = default;
    private float lastJumpTime;
    private bool flying;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        randomMaxSpeed = Random.Range(enemySwarmData.minSpeed, enemySwarmData.maxSpeed);
        waverAmount = Random.Range(enemySwarmData.minWaver, enemySwarmData.maxWaver);
        rotationSpeed = Random.Range(enemySwarmData.minRotationSpeed, enemySwarmData.maxRotationSpeed) * randomMaxSpeed / enemySwarmData.minSpeed;
        reflex = Random.Range(enemySwarmData.minReflex, enemySwarmData.maxReflex);
        timeBetweenJumps = Random.Range(enemySwarmData.minTimeBetweenJumps, enemySwarmData.maxTimeBetweenJumps);
    }

    private void Update()
    {
        if (flying)
        {
            Drag();
            return;
        }
        Vector3 velocity = transform.forward * currentSpeed;
        velocity.y = body.velocity.y - 50 * Time.deltaTime;
        if (Time.time - lastJumpTime > timeBetweenJumps)
        {
            if (Physics.Raycast(transform.position, transform.forward, 1.2f))
            {
                if (Physics.Raycast(transform.position, -transform.up, 1.2f))
                {

                    velocity.y = enemySwarmData.jumpVelocity * 5;
                    lastJumpTime = Time.time;
                }
            }
        }
        body.velocity = velocity;
        if (Time.time % reflex < 0.01)
        {
            directionToTarget = target.position - transform.position;
        }

        float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        //Debug.Log(angle);

        if (isFleeing)
        {
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
        else
        {
            if (directionToTarget.magnitude > enemySwarmData.preferedDistToTarget)
            {
                OuterZone(angle, directionToTarget);
            }
            else
            {
                InnerZone();
            }
        }
    }

    private void Drag()
    {
        Vector3 velocity = body.velocity - body.velocity * (10f * Time.deltaTime);
        velocity.y = body.velocity.y;
        body.velocity = velocity;
    }

    private void OuterZone(float angle, Vector3 dist)
    {
        if (currentSpeed < randomMaxSpeed)
        {
            currentSpeed += enemySwarmData.accel * Time.deltaTime;
        }
        if (Mathf.Abs(angle) < enemySwarmData.lockAngle)
        {
            transform.Rotate(0, waverAmount * Time.deltaTime,0 );
        }
        else if(angle > 0)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }
    }

    public void ExplosionForce(Vector3 hitPos)
    {
        flying = true;
        body.AddExplosionForce(10000, hitPos, 3);
    }

    private void InnerZone()
    {
    }
}