using UnityEngine;
using  UnityEngine.SceneManagement;

public class TutorialGoal : MonoBehaviour
{
    [SerializeField] private MeshRenderer goal;
    // Update is called once per frame

    private void Awake()
    {
        goal.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("PowerUp"))
        {
            goal.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        SceneManager.LoadScene("MainMenu");
    }
}
