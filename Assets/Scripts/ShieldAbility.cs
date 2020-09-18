using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float ShieldUpTime = 5;
    [SerializeField]private float growSpeed;
    private bool shieldActive = default;
    private GameObject shield;
    private float lastShieldUpTime;
    
    private void Update()
    {
        if (Input.GetButtonDown("Shield"))
        {
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
                shield.transform.localScale = Vector3.MoveTowards(shield.transform.localScale, Vector3.one, growSpeed);
            }
            else if(!shieldActive && shield.transform.localScale.x >= 0f)
            {
                shield.transform.localScale = Vector3.MoveTowards(shield.transform.localScale, Vector3.zero, growSpeed);
            }
        }
    }
}
