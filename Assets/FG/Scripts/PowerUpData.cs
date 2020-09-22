using UnityEngine;

namespace FG
{

    [CreateAssetMenu(menuName = "PowerUpData", fileName = "PowerUpData")]
    public class PowerUpData : ScriptableObject
    {
        [Header("General")] public LayerMask raycastMask;

        [Header("Rocket Weapon")] public int numberOfRockets = 1;
        public float rocketFireRate = 1f;
        public float rocketDamage = 1f;
        public float rocketSpeed = 1f;
        public float rocketLiftOffThrust = 100f;
        public float rocketLiftOffTime = 3f;
        public float rocketExplosionRadius = 5;
        public float rocketExplosionForce = 100;
        public ObjectPooler.ObjectType rocketType;
    }
}
