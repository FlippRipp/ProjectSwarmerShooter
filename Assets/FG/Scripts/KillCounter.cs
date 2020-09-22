using UnityEngine;
using UnityEngine.UI;

namespace FG
{

    public class KillCounter : MonoBehaviour
    {
        [SerializeField] private Text killTracker;
        private int kills;
        private void Awake()
        {
            GameplayEventManager.instance.OnKill += OnKill;
        }

        private void OnKill()
        {
            kills++;
            if (killTracker)
            {
                killTracker.text = kills.ToString();
            }
        }
    }
}
