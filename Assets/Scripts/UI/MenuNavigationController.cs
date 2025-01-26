using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigationController : MonoBehaviour
{
    [SerializeField]
    private Selectable[] focusableUIElements;

    [SerializeField]
    private int indexOfCurrentlyFocusedUIElement = -1; //-1 means nothing is focused

    [SerializeField]
    private float duplicateInputCooldown = 0f;
    private float currentNavigateCooldown = 0f;
    private float currentSubmitCooldown = 0f;

    private InputAction navigateAction;
    private InputAction submitAction;

    void Start()
    {
        this.navigateAction = InputSystem.actions.FindAction("Navigate");
        this.submitAction = InputSystem.actions.FindAction("Submit");

        this.navigateAction.performed += OnNavigate;
        this.submitAction.performed += OnSubmit;
    }

    private void OnDestroy()
    {
        this.navigateAction.performed -= OnNavigate;
        this.submitAction.performed -= OnSubmit;
    }

    private void OnEnable()
    {
        this.indexOfCurrentlyFocusedUIElement = 0;
        this.focusableUIElements.ElementAtOrDefault(this.indexOfCurrentlyFocusedUIElement)?.Select();
    }

    private void OnDisable()
    {
        this.indexOfCurrentlyFocusedUIElement = -1;
    }

    private void OnNavigate(InputAction.CallbackContext callbackContext)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }

        Vector2 navigateInput = callbackContext.ReadValue<Vector2>();
        if (navigateInput.y > 0.5f)
        {
            // navigate upwards
            --this.indexOfCurrentlyFocusedUIElement;
            this.currentNavigateCooldown = this.duplicateInputCooldown;
        }
        else if (navigateInput.y < -0.5f)
        {
            // navigate downwards
            ++this.indexOfCurrentlyFocusedUIElement;
            this.currentNavigateCooldown = this.duplicateInputCooldown;
        }
        else
        {
            return;
        }

        this.indexOfCurrentlyFocusedUIElement = Mathf.Clamp(this.indexOfCurrentlyFocusedUIElement, 0, this.focusableUIElements.Length - 1);

        this.focusableUIElements.ElementAtOrDefault(this.indexOfCurrentlyFocusedUIElement)?.Select();
    }

    private void OnSubmit(InputAction.CallbackContext callbackContext)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }

        float submitInput = callbackContext.ReadValue<float>();
        if (submitInput > 0.5f)
        {
            this.currentSubmitCooldown = this.duplicateInputCooldown;
            switch (this.focusableUIElements.ElementAtOrDefault(this.indexOfCurrentlyFocusedUIElement))
            {
                case Button button:
                    button.onClick.Invoke();
                    break;
                case Toggle toggle:
                    toggle.isOn = !toggle.isOn;
                    break;
            }
        }
    }
}
