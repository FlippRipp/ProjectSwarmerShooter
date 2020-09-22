using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

namespace FG
{

    public class DeathMenu : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private GameObject menu;

        private void Awake()
        {
            restartButton.onClick.AddListener(Restart);
            GameplayEventManager.instance.OnEndGame += EnableMenu;
        }

        private void EnableMenu()
        {
            menu.SetActive(true);
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
    }
}
