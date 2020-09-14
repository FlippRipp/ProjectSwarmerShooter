using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swarmer : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float lockAngle;
    [SerializeField] private Transform target;
    [SerializeField] private float preferedDistToTarget = 10f;
    [SerializeField] private float minRotationSpeed = 3f;
    [SerializeField] private float maxRotationSpeed = 10f;
    [SerializeField] private float minReflex = 0.1f;
    [SerializeField] private float maxReflex = 0.1f;
    [SerializeField] private float minTimeBetweenJumps = 1;
    [SerializeField] private float maxTimeBetweenJumps = 3;
    [SerializeField] private float jumpVelocity;
    public float maxWaver = 5f;
    public float minWaver = -5f;
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
        randomMaxSpeed = Random.Range(minSpeed, maxSpeed);
        waverAmount = Random.Range(minWaver, maxWaver);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed) * randomMaxSpeed / minSpeed;
        reflex = Random.Range(minReflex, maxReflex);
        timeBetweenJumps = Random.Range(minTimeBetweenJumps, maxTimeBetweenJumps);
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
            Debug.Log("time to jump");
            if (Physics.Raycast(transform.position, transform.forward, 1.2f))
            {
                if (Physics.Raycast(transform.position, -transform.up, 1.2f))
                {

                    Debug.Log("boing");
                    velocity.y = jumpVelocity * 5;
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
            if (directionToTarget.magnitude > preferedDistToTarget)
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
            currentSpeed += accel * Time.deltaTime;
        }
        if (Mathf.Abs(angle) < lockAngle)
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
        body.AddExplosionForce(1000, hitPos, 3);
    }

    private void InnerZone()
    {
    }
}