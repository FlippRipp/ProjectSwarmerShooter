using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShotWeapon : Weapon
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

            if (weaponData.bulletPrefab)
            {
                GameObject bullet = Instantiate(weaponData.bulletPrefab, weaponTransform.position, Quaternion.identity);
                bullet.transform.LookAt(lookPoint);
                bullet.transform.Rotate(new Vector3(0, angle, 0));
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1800);
            }
        }
    }
}
