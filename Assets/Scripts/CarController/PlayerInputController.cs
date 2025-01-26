using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : InputController
{
    [SerializeField]
    private PauseMenuManager pauseMenuManager;

    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction decelerateAction;
    private InputAction boostAction;
    private InputAction handbrakeAction;
    private InputAction pauseAction;


    private void Awake()
    {
        PlayerName = DriverNames.GetPlayerDriverName();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.steerAction = InputSystem.actions.FindAction("Steer");
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
        this.decelerateAction = InputSystem.actions.FindAction("Decelerate");
        this.boostAction = InputSystem.actions.FindAction("Boost");
        this.handbrakeAction = InputSystem.actions.FindAction("Handbrake");

        this.pauseAction = InputSystem.actions.FindAction("Pause");
        this.pauseAction.performed += (_) =>
        {
            if (pauseMenuManager != null)
            {
                pauseMenuManager.gameObject.SetActive(true);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs();
    }

    private void ProcessInputs()
    {
        // steer
        Vector2 steerInput = this.steerAction.ReadValue<Vector2>();
        this.currentSteerRequest = new Vector3(
            SettingsManager.Instance.IsPlayerPitchInputInverted
                ? steerInput.y
                : -steerInput.y,
            steerInput.x,
            0
        );

        // accelerate
        float accelerationInput = this.accelerateAction.ReadValue<float>();
        this.decelerationInput = this.decelerateAction.ReadValue<float>();

        this.boostInput = this.boostAction.ReadValue<float>();

        this.accelerationRequest = accelerationInput - decelerationInput;

        this.isHandbrakeOn = Mathf.Approximately(1, this.handbrakeAction.ReadValue<float>());
    }
}
