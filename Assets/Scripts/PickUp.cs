using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{

    public class PickUp : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameplayEventManager.instance.PickUp();
            }
        }
    }
}
