using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    private bool shieldActive = default;
    private GameObject shield;
    [SerializeField]private float growSpeed;
    
    private void Update()
    {
        if (Input.GetButtonDown("Shield"))
        {
            shieldActive = true;
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

        if (shield)
        {
            if (shieldActive && shield.transform.localScale.x <= 1f)
            {
                shield.transform.localScale = Vector3.MoveTowards(shield.transform.localScale, Vector3.one, growSpeed);
            }
        }
    }
}
