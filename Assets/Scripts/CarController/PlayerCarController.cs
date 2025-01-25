using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField]
    Rigidbody carRigidBody;

    [SerializeField]
    private float maxSpeedWithoutBoostMph = 100;

    [SerializeField]
    private float accelerationRate = 10;

    private float currentSpeedRequest;

    private float currentSpeed = 0;
    private Vector3 currentVelocity;

    private InputAction accelerateAction;

    // Start is called before the first frame update
    void Start()
    {
        this.accelerateAction = InputSystem.actions.FindAction("Accelerate");
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs();        
    }

    private void FixedUpdate()
    {
        this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.currentSpeedRequest, Time.fixedDeltaTime * this.accelerationRate);
        this.currentVelocity = this.currentSpeed * this.transform.forward;
        this.carRigidBody.AddForce(this.currentVelocity, ForceMode.Acceleration);
    }

    private void ProcessInputs()
    {
        float accelerationRequest = this.accelerateAction.ReadValue<float>();
        this.currentSpeedRequest = this.maxSpeedWithoutBoostMph * accelerationRequest;
    }
}
