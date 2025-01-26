using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MenuManager
{
    [Header("Pause")]
    [SerializeField]
    private TimerDisplay timerDisplay;

    [Header("Exit")]
    [SerializeField]
    private Object sceneToLoad;

    private InputController[] inputControllers;

    private void Start()
    {
        if (this.inputControllers?.Any() != true)
        {
            inputControllers = FindObjectsOfType<InputController>();
        }

        timerDisplay = timerDisplay ?? FindObjectOfType<TimerDisplay>();
    }

    private void OnEnable()
    {
        InputSystem.actions.FindAction("Pause").performed += OnPauseInput;
        foreach (InputController inputController in this.inputControllers ?? (this.inputControllers = FindObjectsOfType<InputController>()))
        {
            inputController.enabled = false;
        }

        if (timerDisplay != null)
        {
            timerDisplay.enabled = false;
        }
    }

    public override void PlayGame()
    {
        InputSystem.actions.FindAction("Pause").performed -= OnPauseInput;
        foreach (InputController inputController in this.inputControllers ?? (this.inputControllers = FindObjectsOfType<InputController>()))
        {
            if (inputController != null)
            {
                inputController.enabled = true;
            }
        }
        if (timerDisplay != null)
        {
            timerDisplay.enabled = true;
        }
        this.gameObject.SetActive(false);
    }

    public override void Exit() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPauseInput(InputAction.CallbackContext callbackContext)
    {
        this.PlayGame();
    }
}
