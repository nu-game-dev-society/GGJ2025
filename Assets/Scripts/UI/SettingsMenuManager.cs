using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject previousMenu;

    [SerializeField]
    private Toggle invertPitchToggle;

    [SerializeField]
    private Button exitSettingsButton;


    private void Start()
    {
        this.invertPitchToggle.onValueChanged.AddListener(this.OnInvertPitchToggleClicked);
        this.exitSettingsButton.onClick.AddListener(this.ExitSettings);
    }

    private void OnInvertPitchToggleClicked(bool newValue)
    {
        SettingsManager.Instance.IsPlayerPitchInputInverted = newValue;
    }

    private void ExitSettings()
    {
        this.previousMenu.SetActive(true);
        this.gameObject.SetActive(false);
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
