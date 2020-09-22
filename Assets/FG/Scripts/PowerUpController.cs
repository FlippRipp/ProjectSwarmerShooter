using UnityEngine;

namespace FG
{
    public class PowerUpController : WeaponActivator
    {
        private PowerUpData powerUpData;
        [SerializeField] private int powerUpCharges = 10;
        
        [SerializeField] private Weapon weapon;

        private void Awake()
        {
            GameplayEventManager.instance.OnPickUp += OnPickUp;
            ActiveWeapons[0] = weapon;
        }

        private void OnPickUp()
        {
            powerUpCharges++;
        }

        private void Update()
        {
            if (Input.GetButtonDown("PowerUp") && powerUpCharges > 0)
            {
                powerUpCharges--;
                GameplayEventManager.instance.PowerUpUsed();
                FireWeapon(transform.forward);
            }
        }
    }
}
