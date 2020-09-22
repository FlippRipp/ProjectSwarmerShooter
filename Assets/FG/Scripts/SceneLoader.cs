using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] private string sceneName;
    
    public void LoadGame(string sceneToLoad)
    {
        StartCoroutine(DelayedLoadGame(sceneToLoad));
    }

    private IEnumerator DelayedLoadGame(string sceneToLoad)
    {
        yield return new WaitForSeconds(1f);
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
