using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
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
    private bool isPitchControlInverted = true;

    private Vector3 currentSteerRequest;
    private float currentSpeedRequest;

    private float currentSpeed = 0;
    private Vector3 currentVelocity;

    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction decelerateAction;
    private InputAction boostAction;

    // Start is called before the first frame update
    void Start()
    {
        this.steerAction = InputSystem.actions.FindAction("Steer");
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
        this.decelerateAction = InputSystem.actions.FindAction("Decelerate");
        this.boostAction = InputSystem.actions.FindAction("Boost");

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
        this.carRigidBody.AddRelativeTorque(this.currentSteerRequest);
        this.carRigidBody.angularDrag = this.currentSteerRequest == Vector3.zero
            ? this.angularDragWhenIdle
            : this.angularDragWhenMoving;

        // accelerate
        this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.currentSpeedRequest, Time.fixedDeltaTime * this.accelerationRate);
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
        float accelerationRequest = this.accelerateAction.ReadValue<float>() - this.decelerateAction.ReadValue<float>();

        float boostInput = this.boostAction.ReadValue<float>();
        float boostSpeed = boostInput * this.boostSpeedModifier;
        float boostAcceleration = boostInput * this.boostAccelerationModifier;

        float maxSpeed = this.maxSpeedWithoutBoost + boostSpeed;
        accelerationRequest += boostAcceleration;

        this.currentSpeedRequest =  maxSpeed * accelerationRequest;
    }
}
