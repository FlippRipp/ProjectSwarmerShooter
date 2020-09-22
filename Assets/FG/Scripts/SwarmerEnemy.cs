using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FG
{

    public class SwarmerEnemy : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private EnemySwarmData enemySwarmData;

        [NonSerialized]
        public Rigidbody body;
        private float waverAmount = 10;
        private float randomMaxSpeed;
        private float rotationSpeed;
        private float currentSpeed;
        private float reflex;
        private Vector3 directionToTarget;
        private float timeBetweenJumps;
        private float lastJumpTime;
        private bool isDead;
        private bool isSeen;
        private float lastAttackTime;
        private float lastTimeSeen;
        private float dragOnGround;
        private RigidbodyConstraints constraints;
        public event Action<SwarmerEnemy> OnDeath;


        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            dragOnGround = body.drag;
            constraints = body.constraints;
            randomMaxSpeed = Random.Range(enemySwarmData.minSpeed, enemySwarmData.maxSpeed);
            waverAmount = Random.Range(enemySwarmData.minWaver, enemySwarmData.maxWaver);
            rotationSpeed = Random.Range(enemySwarmData.minRotationSpeed, enemySwarmData.maxRotationSpeed) *
                randomMaxSpeed / enemySwarmData.minSpeed;
            reflex = Random.Range(enemySwarmData.minReflex, enemySwarmData.maxReflex);
            timeBetweenJumps = Random.Range(enemySwarmData.minTimeBetweenJumps, enemySwarmData.maxTimeBetweenJumps);
        }

        private void Update()
        {
            DespawnEnemy();
            if (isDead)
            {
                return;
            }

            Move();
        }

        private void OnEnable()
        {
            lastTimeSeen = Time.time;
            body.constraints = constraints;
            body.drag = dragOnGround;
            isDead = false;
            transform.rotation = Quaternion.identity;
        }

        private void Move()
        {
            Vector3 velocity = transform.forward * currentSpeed;
            velocity.y = body.velocity.y - 50 * Time.deltaTime;
            if (Time.time - lastJumpTime > timeBetweenJumps)
            {
                if (Physics.Raycast(transform.position, transform.forward, 1.2f * transform.localScale.x))
                {
                    if (Physics.Raycast(transform.position, -transform.up, 0.6f * transform.localScale.y))
                    {

                        velocity.y = enemySwarmData.jumpVelocity * 5;
                        lastJumpTime = Time.time;
                    }
                }
            }

            body.velocity = velocity;
            if (Time.time % reflex < 0.01f)
            {
                directionToTarget = target.position - transform.position;
            }

            float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);


                if (directionToTarget.magnitude > enemySwarmData.preferedDistToTarget)
                {
                    OuterZone(angle);
                }
        }

        private void DespawnEnemy()
        {
            if (isSeen) return;
            if (isDead && Time.time - lastTimeSeen > enemySwarmData.bodyDeSpawnTime)
            {
                gameObject.SetActive(false);
            }
            else if (Time.time - lastTimeSeen > enemySwarmData.unseenDeSpawnTime)
            {
                OnDeath?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        private void OnBecameInvisible()
        {
            isSeen = false;
            if (!target) return;
            Vector3 dir = target.position - transform.position;
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, dir, out hit, 100)) return;
            if (!hit.collider.CompareTag("Player")) return;
            lastTimeSeen = Time.time;
        }

        private void OnBecameVisible()
        {
            isSeen = true;
        }

        public void Death()
        {
            if(isDead) return;
            GameplayEventManager.instance.Kill();
            body.constraints = RigidbodyConstraints.None;
            isDead = true;
            OnDeath?.Invoke(this);
        }

        private void OuterZone(float angle)
        {
            if (currentSpeed < randomMaxSpeed)
            {
                currentSpeed += enemySwarmData.accel * Time.deltaTime;
            }

            if (Mathf.Abs(angle) < enemySwarmData.lockAngle)
            {
                transform.Rotate(0, waverAmount * Time.deltaTime, 0);
            }
            else if (angle > 0)
            {
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && !isDead)
            {
                DamageTarget(other.gameObject);
            }
        }
        
        private void DamageTarget(GameObject targetToDamage)
        {
            if (Time.time - lastAttackTime > enemySwarmData.damageTime)
            {
                CharacterHealth characterHealth = targetToDamage.GetComponent<CharacterHealth>();
                if (characterHealth)
                {
                    characterHealth.TakeDamage(enemySwarmData.damage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}