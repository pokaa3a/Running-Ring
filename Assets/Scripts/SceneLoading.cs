using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
