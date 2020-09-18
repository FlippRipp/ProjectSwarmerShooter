using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        StartCoroutine(DelayedLoadGame());
    }

    public IEnumerator DelayedLoadGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
