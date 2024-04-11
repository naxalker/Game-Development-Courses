using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static AsyncProcessor _processor;

    public static void SetProcessor(AsyncProcessor processor)
    {
        _processor = processor;
    }

    public static void Load(Scene targetScene)
    {
        if (_processor == null)
        {
            Debug.LogError("AsyncProcessor not set in Loader.");
            return;
        }

        SceneManager.LoadScene(Scene.LoadingScene.ToString());

        _processor.StartCoroutine(LoadSceneAsync(targetScene));
    }

    private static IEnumerator LoadSceneAsync(Scene targetScene)
    {
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
