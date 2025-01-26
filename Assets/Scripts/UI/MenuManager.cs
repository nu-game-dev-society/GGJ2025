using UnityEngine;
using UnityEngine.UI;

public abstract class MenuManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    protected GameObject settingsMenu;

    private void Start()
    {
        this.settingsMenu.SetActive(false);
    }

    public abstract void PlayGame();

    public void ShowSettingsMenu()
    {
        this.gameObject.SetActive(false);
        this.settingsMenu.SetActive(true);
    }

    public abstract void Exit();
}
