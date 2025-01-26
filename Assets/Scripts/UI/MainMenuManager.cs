using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Play game")]
    [SerializeField]
    private Button playGameButton;

    [SerializeField]
    private Object sceneToLoad;

    [Header("Settings")]
    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private Button exitSettingsButton;

    [Header("Quit")]
    [SerializeField]
    private Button quitButton;


    private void Start()
    {
        this.playGameButton.onClick.AddListener(this.PlayGame);
        this.settingsButton.onClick.AddListener(this.ShowSettingsMenu);
        this.quitButton.onClick.AddListener(this.Quit);
        this.exitSettingsButton.onClick.AddListener(this.ExitSettings);
        this.settingsMenu.SetActive(false);
    }

    private void PlayGame()
    {
        if (sceneToLoad is SceneAsset sceneToLoadAsSceneAsset)
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    private void ShowSettingsMenu()
    {
        this.gameObject.SetActive(false);
        this.settingsMenu.SetActive(true);
    }

    private void ExitSettings()
    {
        this.gameObject.SetActive(true);
        this.settingsMenu.SetActive(false);
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
 