using System;
using UnityEngine;

namespace FG
{
    [DefaultExecutionOrder(-100)]
    public class GameplayEventManager : MonoBehaviour
    {

        public static GameplayEventManager instance;
        public event Action OnPickUp;
        public event Action OnKill;
        public event Action OnShield;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PickUp()
        {
            OnPickUp?.Invoke();
        }

        public void Kill()
        {
            OnKill?.Invoke();
        }

        public void ShieldActive()
        {
            
        }
    }
}
