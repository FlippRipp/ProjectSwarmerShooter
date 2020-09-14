
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("General")]
    public LayerMask raycastMask;

    [Header("Split Shot Weapon")]
    public int numberOfBullets = 1;
    public float bulletFireRate = 1f;
    public float bulletSize = 1f;
    public float bulletDamage = 1f;
    public float bulletSpread = 10f;
    public GameObject bulletPrefab;
    
    [Header("Rocket Weapon")]
    public int numberOfRockets = 1;
    public float rocketFireRate = 1f;
    public float rocketDamage = 1f;
    public float rocketSpeed = 1f;
    public float rocketLiftOffThrust = 100f;
    public float rocketLiftOffTime = 3f;
    public GameObject rocketPrefab;

}
