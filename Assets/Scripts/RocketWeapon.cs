using UnityEngine;

public class RocketWeapon : Weapon
{
    private float lastRocketFireTime;

    public override void Fire(WeaponData weaponData, Transform weaponTransform, Transform activator, Vector3 attackDir)
    {
        if (Time.time - lastRocketFireTime > weaponData.rocketFireRate)
        {
            lastRocketFireTime = Time.time;
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

        for (int i = 0; i < weaponData.numberOfRockets; i++)
        {
            if (weaponData.rocketPrefab)
            {
                GameObject rocket = Instantiate(weaponData.rocketPrefab, weaponTransform.position, Quaternion.identity);
                rocket.GetComponent<Rocket>().Init(weaponData.rocketLiftOffTime, weaponData.rocketDamage,
                    weaponData.rocketSpeed, weaponData.rocketLiftOffThrust, hit.transform, activator, hit.point);
            }
        }

    }
}
