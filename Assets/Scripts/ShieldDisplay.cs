using UnityEngine;
using  UnityEngine.UI;

namespace FG
{
    public class ShieldDisplay : MonoBehaviour
    {
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private GameObject shieldOutline;


        private void Awake()
        {
            GameplayEventManager.instance.OnShieldChargeChanged += UpdateSlider;
        }

        private void UpdateSlider(float value)
        {
            shieldSlider.value = value;
            shieldOutline.SetActive(value >= 100);
        }
    }
}
