
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeapon : WeaponActivator
{
    [SerializeField] private Weapon[] weapons;
    private void Awake()
    {
        ActiveWeapons = new Weapon[2];
        GameplayEventManager.current.onPickUp += OnPickUp;
        EquipRandomWeapon(0);
        EquipRandomWeapon(1);
    }

    private void OnPickUp()
    {
        
    }

    private void EquipRandomWeapon(int equipSlot)
    {
        int randomWeapon = Random.Range(0, weapons.Length);
        print(randomWeapon);
        if (weapons[randomWeapon] != null)
        {
            ActiveWeapons[equipSlot] = weapons[randomWeapon];
        }
    }
    
    private void EquipRandomWeapon()
    {
        int randomWeapon = Random.Range(0, weapons.Length);
        if (weapons[randomWeapon] != null)
        {
            int randomWeaponSlot = Random.Range(0, ActiveWeapons.Length - 1);
            ActiveWeapons[randomWeaponSlot] = weapons[randomWeapon];
        }
    }


    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            FireWeapon(transform.forward, 0);
        }

        if (Input.GetButton("Fire2"))
        {
            FireWeapon(transform.forward, 1);
        }
    }
}
