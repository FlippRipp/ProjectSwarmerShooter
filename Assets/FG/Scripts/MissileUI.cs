using UnityEngine;
using UnityEngine.UI;

namespace FG
{

    public class MissileUI : MonoBehaviour
    {

        private int rockets;

        [SerializeField] private Text rocketNumber;
        [SerializeField] private GameObject rocketUIVisuals;

        private void Awake()
        {
            rocketUIVisuals.SetActive(false);
            GameplayEventManager.instance.OnPowerUpUsed += RemoveRocket;
            GameplayEventManager.instance.OnPickUp += AddRocket;
        }

        private void UpdateUI()
        {
            if (rockets == 0)
            {
                rocketUIVisuals.SetActive(false);
            }
            else
            {
                rocketUIVisuals.SetActive(true);
                rocketNumber.text = rockets.ToString();
            }
        }

        private void AddRocket()
        {
            rockets++;
            UpdateUI();
        }

        private void RemoveRocket()
        {
            rockets--;
            UpdateUI();
        }
    }
}
