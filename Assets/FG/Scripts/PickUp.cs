using UnityEngine;

namespace FG
{

    public class PickUp : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameplayEventManager.instance.PickUp();
            gameObject.SetActive(false);
        }
    }
}
