using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigationController : MonoBehaviour
{
    [SerializeField]
    private Button[] focusableUIElements;

    [SerializeField]
    private int indexOfCurrentlyFocusedUIElement;

    [SerializeField]
    private float navigateCooldown = 0.5f;
    private float currentNavigateCooldown = 0f;

    private InputAction navigateAction;
    private InputAction submitAction;

    // Start is called before the first frame update
    void Start()
    {
        this.navigateAction = InputSystem.actions.FindAction("Navigate");
        this.submitAction = InputSystem.actions.FindAction("Submit");
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessNavigation();
        this.ProcessSubmit();
    }

    private void ProcessNavigation()
    {
        if (currentNavigateCooldown > 0f)
        {
            this.currentNavigateCooldown -= Time.deltaTime;
            return;
        }

        Vector2 navigateInput = this.navigateAction.ReadValue<Vector2>();
        if (navigateInput.y > 0.5f)
        {
            // navigate upwards
            --this.indexOfCurrentlyFocusedUIElement;
            this.currentNavigateCooldown = this.navigateCooldown;
        }
        else if (navigateInput.y < -0.5f)
        {
            // navigate downwards
            ++this.indexOfCurrentlyFocusedUIElement;
            this.currentNavigateCooldown = this.navigateCooldown;
        }

        this.indexOfCurrentlyFocusedUIElement = Mathf.Clamp(this.indexOfCurrentlyFocusedUIElement, 0, this.focusableUIElements.Length - 1);

        this.focusableUIElements[this.indexOfCurrentlyFocusedUIElement].Select();
    }

    private void ProcessSubmit()
    {
        float submitInput = this.submitAction.ReadValue<float>();
        if (submitInput > 0.5f)
        {
            this.focusableUIElements[this.indexOfCurrentlyFocusedUIElement].onClick.Invoke();
        }
    }
}
