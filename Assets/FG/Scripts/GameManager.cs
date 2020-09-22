using UnityEngine;

namespace FG
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static Camera PlayerCamera { get; private set; }
        public static Transform PlayerTransform { get; private set; }
        public static Transform PlayerCameraTransform { get; private set; }

        public static bool LockCursor
        {
            set
            {
                if (value)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        private void Awake()
        {
            PlayerCamera = Camera.main;
            PlayerTransform = GameObject.FindWithTag("Player").transform;
            PlayerCameraTransform = PlayerCamera.transform;
            #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0; // VSync must be disabled
            Application.targetFrameRate = 60;
            #endif
        }

        public void EndGame()
        {
            GameplayEventManager.instance.EndGame();
            LockCursor = false;
            Time.timeScale = 0;
        }


        private void OnEnable()
        {
            LockCursor = true;
        }

        private void OnDisable()
        {
            LockCursor = false;
        }
    }
}