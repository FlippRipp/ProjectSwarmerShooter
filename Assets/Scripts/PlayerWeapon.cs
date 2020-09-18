
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
        //GameplayEventManager.current.onPickUp += OnPickUp;
        EquipRandomWeapon(0);
        EquipRandomWeapon(1);
    }

    private void OnPickUp()
    {
        
    }

    private void EquipRandomWeapon(int equipSlot)
    {
        int randomWeapon = Random.Range(0, weapons.Length);
        if (weapons[randomWeapon] != null)
        {
            if (equipSlot == 0)
            {
                if (ActiveWeapons[1] != weapons[randomWeapon])
                {
                    ActiveWeapons[equipSlot] = weapons[randomWeapon];
                }
                else
                {
                    EquipRandomWeapon(equipSlot);
                }
            }
            else
            {
                if (ActiveWeapons[0] != weapons[randomWeapon])
                {
                    ActiveWeapons[equipSlot] = weapons[randomWeapon];
                }
                else
                {
                    EquipRandomWeapon(equipSlot);
                }
            }
        }
    }
    
    private void EquipRandomWeapon()
    {
        int randomWeapon = Random.Range(0, weapons.Length);
        if (weapons[randomWeapon] != null)
        {
            int randomWeaponSlot = Random.Range(0, ActiveWeapons.Length - 1);
            if (randomWeaponSlot == 0)
            {
                if (ActiveWeapons[1] == weapons[randomWeapon])
                {
                    ActiveWeapons[randomWeaponSlot] = weapons[randomWeapon];
                }
                else
                {
                    EquipRandomWeapon();
                }
            }
            else
            {
                if (ActiveWeapons[0] == weapons[randomWeapon])
                {
                    ActiveWeapons[randomWeaponSlot] = weapons[randomWeapon];
                }
                else
                {
                    EquipRandomWeapon();
                }
            }
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
