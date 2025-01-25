using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : InputController
{
    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction decelerateAction;
    private InputAction boostAction;
    private InputAction handbrakeAction;


    private void Awake()
    {
        PlayerName = DriverNames.GetPlayerDriverName();
        Debug.LogWarning($"set {PlayerName}");
    }
    // Start is called before the first frame update
    void Start()
    {
        this.steerAction = InputSystem.actions.FindAction("Steer");
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
        this.decelerateAction = InputSystem.actions.FindAction("Decelerate");
        this.boostAction = InputSystem.actions.FindAction("Boost");
        this.handbrakeAction = InputSystem.actions.FindAction("Handbrake");
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

        this.accelerationRequest = boostInput + accelerationInput - decelerationInput;

        this.isHandbrakeOn = Mathf.Approximately(1, this.handbrakeAction.ReadValue<float>());
    }
}
