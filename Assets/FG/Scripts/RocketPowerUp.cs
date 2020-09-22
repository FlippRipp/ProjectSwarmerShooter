using UnityEngine;

namespace FG
{
    public class RocketPowerUp : Weapon
    {
        private float lastRocketFireTime;
        [SerializeField] private PowerUpData powerUpData;


        public override void Fire(Transform weaponTransform, Transform activator, Vector3 attackDir)
        {
            if (Time.time - lastRocketFireTime > powerUpData.rocketFireRate)
            {
                lastRocketFireTime = Time.time;
            }
            else
            {
                return;
            }

            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 10000, powerUpData.raycastMask);


            for (int i = 0; i < powerUpData.numberOfRockets; i++)
            {
                GameObject rocket = ObjectPooler.instance.GetPooledObject(powerUpData.rocketType);
                if (rocket)
                {
                    rocket.transform.position = weaponTransform.position;
                    rocket.GetComponent<Rocket>().Init(powerUpData, activator, hit.transform, hit.point);
                }

            }
        }
    }
}