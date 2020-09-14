using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Fire(WeaponData weaponData, Transform weaponTransform, Transform activator, Vector3 attackDir);
}
