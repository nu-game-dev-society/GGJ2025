using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField]
    Rigidbody carRigidBody;

    [SerializeField]
    private float maxSpeedWithoutBoost = 100;

    [SerializeField]
    private float boostSpeedModifier = 1.5f;

    [SerializeField]
    private float boostAccelerationModifier = 1.5f;

    [SerializeField]
    private float accelerationRate = 10;

    private Vector3 currentSteerRequest;
    private float currentSpeedRequest;

    private float currentSpeed = 0;
    private Vector3 currentVelocity;

    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction boostAction;

    // Start is called before the first frame update
    void Start()
    {
        this.steerAction = InputSystem.actions.FindAction("Steer");
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
        this.boostAction = InputSystem.actions.FindAction("Boost");
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs();        
    }

    private void FixedUpdate()
    {
        // rotate
        
        this.carRigidBody.AddRelativeTorque(this.currentSteerRequest);

        // add force
        this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.currentSpeedRequest, Time.fixedDeltaTime * this.accelerationRate);
        this.currentVelocity = this.currentSpeed * this.transform.forward;
        this.carRigidBody.AddForce(this.currentVelocity, ForceMode.Acceleration);
    }

    private void ProcessInputs()
    {
        // steer
        this.currentSteerRequest = new Vector3(
            this.steerAction.ReadValue<Vector2>().y,
            this.steerAction.ReadValue<Vector2>().x,
            0
        );

        // accelerate
        float accelerationRequest = this.accelerateAction.ReadValue<float>();

        float boostInput = this.boostAction.ReadValue<float>();
        float boostSpeed = boostInput * this.boostSpeedModifier;
        float boostAcceleration = boostInput * this.boostAccelerationModifier;

        float maxSpeed = this.maxSpeedWithoutBoost + boostSpeed;
        accelerationRequest += boostAcceleration;

        this.currentSpeedRequest =  maxSpeed * accelerationRequest;
    }
}
