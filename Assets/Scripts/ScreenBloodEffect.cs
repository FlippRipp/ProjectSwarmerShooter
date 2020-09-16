
using System;
using UnityEngine;

namespace FG
{

    public class ScreenBloodEffect : MonoBehaviour
    {
        
        public GameObject[] bloodEffects;

        private void Awake()
        {
            GameManager.PlayerTransform.GetComponent<CharacterHealth>().CharacterHealthChanged.AddListener(UpdateBloodEffect);
        }

        private void UpdateBloodEffect(float health)
        {
            foreach (GameObject bloodEffect in bloodEffects)
            {
                bloodEffect.SetActive(false);
            }
            if (health <= 0)
            {
                bloodEffects[4].SetActive(true);
            }
            else if (health < 20)
            {
                bloodEffects[3].SetActive(true);
            }

            else if(health < 40)
            {
                bloodEffects[2].SetActive(true);
            }
            
            else if(health < 60)
            {
                bloodEffects[1].SetActive(true);
            }
            
            else if(health < 80)
            {
                bloodEffects[0].SetActive(true);
            }
        }
    }
}
