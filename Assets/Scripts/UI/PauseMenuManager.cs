using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MenuManager
{
    [Header("Pause")]
    [SerializeField]
    private TimerDisplay timerDisplay;

    [Header("Exit")]
    [SerializeField]
    private Object sceneToLoad;

    [SerializeField]
    private CarController[] carControllers;

    private void Start()
    {
        if (this.carControllers?.Any() != true)
        {
            carControllers = FindObjectsOfType<CarController>();
        }

        timerDisplay = timerDisplay ?? FindObjectOfType<TimerDisplay>();
    }

    private void OnEnable()
    {
        InputSystem.actions.FindAction("Pause").performed += OnPauseInput;
        foreach (CarController carController in this.carControllers ?? (this.carControllers = FindObjectsOfType<CarController>()))
        {
            if (carController?.inputs != null)
            {
                carController.inputs.enabled = false;
            }
        }

        if (timerDisplay != null)
        {
            timerDisplay.enabled = false;
        }
    }

    public override void PlayGame()
    {
        InputSystem.actions.FindAction("Pause").performed -= OnPauseInput;
        foreach (CarController carController in this.carControllers ?? (this.carControllers = FindObjectsOfType<CarController>()))
        {
            if (carController?.inputs != null)
            {
                carController.inputs.enabled = true;
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
