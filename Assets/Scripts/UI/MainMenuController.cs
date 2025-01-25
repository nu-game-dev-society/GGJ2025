using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button playGameButton;

    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private Object sceneToLoad;

    private void Start()
    {
        this.playGameButton.onClick.AddListener(this.PlayGame);
        this.quitButton.onClick.AddListener(this.Quit);
    }

    private void PlayGame()
    {
        if (sceneToLoad is SceneAsset sceneToLoadAsSceneAsset)
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
 