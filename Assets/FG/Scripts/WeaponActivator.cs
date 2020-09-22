using UnityEngine;

public class WeaponActivator : MonoBehaviour
{
  protected Weapon[] ActiveWeapons = new Weapon[1];
  [SerializeField] protected Transform weaponTransform;
  [SerializeField] protected Transform weaponActivator;

  protected void FireWeapon(Vector3 dir, int weaponToFire = 0)
  {
    ActiveWeapons[weaponToFire].Fire(weaponTransform, weaponActivator, dir);
  }
}
