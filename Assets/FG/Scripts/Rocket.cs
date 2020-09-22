using UnityEngine;

namespace FG
{

    public class Rocket : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.ObjectType particleEffect;

        private float rocketLiftOffThrust;
        private float rocketSpeed;
        private float rocketLiftOffTime;
        private Vector3 rocketTargetPos;
        private Vector3 rocketTargetDir;
        private Transform rocketTarget;
        private float rocketExplosionRadius;
        private float rocketExplosionForce;

        private Rigidbody body;

        private float timeOnSpawn;
        private bool lockedOnTarget;

        private void OnEnable()
        {
            timeOnSpawn = Time.time;
            if (!body)
            {
                body = GetComponent<Rigidbody>();
            }
            body.velocity = Vector3.zero;
        }

        public void Init(PowerUpData powerUpData, Transform activator, Transform target, Vector3 targetPos)
        {
            rocketTargetDir = Vector3.zero;
            rocketLiftOffTime = powerUpData.rocketLiftOffTime;
            rocketSpeed = powerUpData.rocketSpeed;
            rocketLiftOffThrust = powerUpData.rocketLiftOffThrust;
            rocketTarget = target;
            rocketExplosionForce = powerUpData.rocketExplosionForce;
            rocketExplosionRadius = powerUpData.rocketExplosionRadius;
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
            if (other.gameObject.CompareTag("Player")) return;
            GameObject particle = ObjectPooler.instance.GetPooledObject(particleEffect);
            if (particle)
            {
                particle.transform.position = transform.position;
            }
            EnemyManager.instance.Explosion(other.contacts[0].point, rocketExplosionForce, rocketExplosionRadius);
            gameObject.SetActive(false);
        }
    }
}
