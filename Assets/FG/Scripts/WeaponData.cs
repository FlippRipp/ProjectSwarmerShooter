using UnityEngine;

namespace FG
{

    [CreateAssetMenu(menuName = "WeaponData", fileName = "WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("General")] public LayerMask raycastMask;

        [Header("Split Shot Weapon")] public int numberOfBullets = 1;
        public float bulletFireRate = 1f;
        public float bulletSize = 1f;
        public float bulletDamage = 1f;
        public float bulletSpread = 10f;
        public float bulletVelocity = 20;
        public ObjectPooler.ObjectType bulletType;
    }
}
