using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
    [Header("Aesthetics")]
    [SerializeField]
    private Material brakeLightsMaterial;

    [SerializeField]
    private Animator[] propellorAnimators;

    [Header("Physics")]
    [SerializeField]
    private Rigidbody carRigidBody;

    [SerializeField]
    private float angularDragWhenIdle;
    private float angularDragWhenMoving;

    [Header("Speed")]
    [SerializeField]
    private float maxSpeedWithoutBoost = 100;

    [SerializeField]
    private float boostSpeedModifier = 1.5f;

    [SerializeField]
    private float boostAccelerationModifier = 1.5f;

    [SerializeField]
    private float accelerationRate = 10;

    [SerializeField]
    private float handbrakeWeight = 5f;

    [Header("Steering")]
    [SerializeField]
    private bool isPitchControlInverted = true;

    [SerializeField]
    private Vector3 steerRate = Vector3.one;

    private Vector3 currentSteerRequest;
    private float currentSpeedRequest;
    private bool isHandbrakeOn;

    private float currentSpeed = 0;
    private Vector3 currentVelocity;

    private Vector3 currentSteer;

    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction decelerateAction;
    private InputAction boostAction;
    private InputAction handbrakeAction;

    // Start is called before the first frame update
    void Start()
    {
        this.steerAction = InputSystem.actions.FindAction("Steer");
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
        this.decelerateAction = InputSystem.actions.FindAction("Decelerate");
        this.boostAction = InputSystem.actions.FindAction("Boost");
        this.handbrakeAction = InputSystem.actions.FindAction("Handbrake");

        this.angularDragWhenMoving = this.carRigidBody.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs();
    }

    private void FixedUpdate()
    {
        // steer        
        this.carRigidBody.AddRelativeTorque(Vector3.Scale(this.currentSteerRequest, this.steerRate));
        this.carRigidBody.angularDrag = this.currentSteerRequest == Vector3.zero
            ? this.angularDragWhenIdle
            : this.angularDragWhenMoving;

        // accelerate
        this.currentSpeed = this.isHandbrakeOn
            ? Mathf.Lerp(this.currentSpeed, 0, Time.fixedDeltaTime * this.accelerationRate * this.handbrakeWeight)
            : Mathf.Lerp(this.currentSpeed, this.currentSpeedRequest, Time.fixedDeltaTime * this.accelerationRate);

        this.currentVelocity = this.currentSpeed * this.transform.forward;
        this.carRigidBody.AddForce(this.currentVelocity, ForceMode.Acceleration);
    }

    private void ProcessInputs()
    {
        // steer
        Vector2 steerInput = this.steerAction.ReadValue<Vector2>();
        this.currentSteerRequest = new Vector3(
            isPitchControlInverted
                ? steerInput.y
                : -steerInput.y,
            steerInput.x,
            0
        );

        // accelerate
        float accelerationInput = this.accelerateAction.ReadValue<float>();
        float decelerationInput = this.decelerateAction.ReadValue<float>();
        if (Mathf.Approximately(0, decelerationInput))
        {
            this.brakeLightsMaterial.SetInt("_Emissive", 0);
        }
        else
        {
            this.brakeLightsMaterial.SetInt("_Emissive", 1);
        }

        float accelerationRequest = accelerationInput - decelerationInput;

        float boostInput = this.boostAction.ReadValue<float>();
        float boostSpeed = boostInput * this.boostSpeedModifier;
        float boostAcceleration = boostInput * this.boostAccelerationModifier;

        float maxSpeed = this.maxSpeedWithoutBoost + boostSpeed;
        accelerationRequest += boostAcceleration;

        this.currentSpeedRequest = maxSpeed * accelerationRequest;

        foreach (Animator propellorAnimator in this.propellorAnimators)
        {
            propellorAnimator.SetFloat("Speed", this.currentSpeedRequest * Time.deltaTime * 10);
        }

        this.isHandbrakeOn = Mathf.Approximately(1, this.handbrakeAction.ReadValue<float>());
    }
}
