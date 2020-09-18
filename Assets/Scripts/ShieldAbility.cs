using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{

    public class ShieldAbility : MonoBehaviour
    {
        [SerializeField] private GameObject shieldPrefab;
        [SerializeField] private float ShieldUpTime = 5;
        [SerializeField] private float growSpeed = 0.05f;
        [SerializeField] private float chargeToActivate = 200;
        private bool shieldActive = default;
        private GameObject shield;
        private float lastShieldUpTime;
        private float charge;


        private void Awake()
        {
            GameplayEventManager.instance.OnKill += ChargeShield;
        }

        private void Update()
        {
            Shield();
        }

        private void Shield()
        {
            if (Input.GetButtonDown("Shield") && charge >= chargeToActivate)
            {
                charge -= chargeToActivate;
                UpdateShieldUI();
                shieldActive = true;
                lastShieldUpTime = Time.time;
                if (!shield)
                {
                    shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    shield.transform.position = transform.position;
                }

                shield.transform.localScale = Vector3.zero;
            }

            if (Time.time - lastShieldUpTime > ShieldUpTime && shieldActive)
            {
                shieldActive = false;
            }

            if (shield)
            {
                if (shieldActive && shield.transform.localScale.x <= 1f)
                {
                    shield.transform.localScale =
                        Vector3.MoveTowards(shield.transform.localScale, Vector3.one, growSpeed);
                }
                else if (!shieldActive && shield.transform.localScale.x >= 0f)
                {
                    shield.transform.localScale =
                        Vector3.MoveTowards(shield.transform.localScale, Vector3.zero, growSpeed);
                }
            }
        }

        private void UpdateShieldUI()
        {
            GameplayEventManager.instance.ShieldChargeChanged(charge / chargeToActivate * 100);
        }

        private void ChargeShield()
        {
            UpdateShieldUI();
            charge++;
        }
    }
}
