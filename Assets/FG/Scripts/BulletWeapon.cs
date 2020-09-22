using UnityEngine;

namespace FG
{
    public class BulletWeapon : Weapon
    {
        private float lastBulletFireTime;
        [SerializeField] private WeaponData weaponData;

        public override void Fire(Transform weaponTransform, Transform activator, Vector3 attackDir)
        {
            if (Time.time - lastBulletFireTime > weaponData.bulletFireRate)
            {
                lastBulletFireTime = Time.time;
            }
            else
            {
                return;
            }

            RaycastHit hit;
            Vector3 lookPoint = weaponTransform.forward;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 10000, weaponData.raycastMask))
            {
                lookPoint = hit.point;
            }

            for (int i = 0; i < weaponData.numberOfBullets; i++)
            {
                float angle = (-(weaponData.numberOfBullets - 1f) / 2f + i) * weaponData.bulletSpread;

                    GameObject bullet = ObjectPooler.instance.GetPooledObject(weaponData.bulletType);
                    if (!bullet)
                    {
                        Debug.Log("ObjectPool did not return a GameObject of type " + ObjectPooler.ObjectType.Bullet);
                        return;
                    }

                    Transform bulletTransform = bullet.transform;
                    bulletTransform.position = weaponTransform.position;
                    bulletTransform.LookAt(lookPoint);
                    bulletTransform.Rotate(new Vector3(0, angle, 0));
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weaponData.bulletVelocity;
            }
        }
    }
}
