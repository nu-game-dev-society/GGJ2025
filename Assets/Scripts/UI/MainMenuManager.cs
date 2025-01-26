using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MenuManager
{
    [SerializeField] 
    private Object sceneToLoad;

    public override void PlayGame()
    { 
        SceneManager.LoadScene("MainScene");
    }

    public override void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
 