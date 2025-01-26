using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Aesthetics")]
    [SerializeField]
    private MeshRenderer bodyRenderer;
    private MaterialPropertyBlock breakLightsMatProps;

    [SerializeField]
    private Animator[] propellorAnimators;

    [SerializeField]
    private Transform[] propellorArms;

    [SerializeField]
    private ParticleSystem boostParticleSystem;

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
    private float maxBoostTimeInSeconds = 1f;
    private float remainingBoostTimeInSeconds = 1f;

    [SerializeField]
    private float accelerationRate = 10;

    [SerializeField]
    private float handbrakeWeight = 5f;

    [SerializeField]
    private Vector3 steerRate = Vector3.one;


    private float currentSpeedRequest = 0;

    private float currentSpeed = 0;
    private Vector3 currentVelocity;


    [SerializeField]
    private InputController inputs;


    [Header("AUDIO")]
    public AudioSource propellorAudioSource; 

    // Start is called before the first frame update
    void Start()
    {
        this.angularDragWhenMoving = this.carRigidBody.angularDrag;

        breakLightsMatProps = new MaterialPropertyBlock();
        this.bodyRenderer.GetPropertyBlock(breakLightsMatProps, 1);

    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs(); 

        UpdatePropellorRotation(inputs.accelerationRequest, inputs.currentSteerRequest.y);
    }

    private void FixedUpdate() 
    {
        Debug.DrawRay(transform.position, inputs.currentSteerRequest, Color.red);

        // steer        
        this.carRigidBody.AddRelativeTorque(Vector3.Scale(inputs.currentSteerRequest, this.steerRate));
        this.carRigidBody.angularDrag = inputs.currentSteerRequest == Vector3.zero
            ? this.angularDragWhenIdle
            : this.angularDragWhenMoving;

        // accelerate
        this.currentSpeed = inputs.isHandbrakeOn
            ? Mathf.Lerp(this.currentSpeed, 0, Time.fixedDeltaTime * this.accelerationRate * this.handbrakeWeight)
            : Mathf.Lerp(this.currentSpeed, this.currentSpeedRequest, Time.fixedDeltaTime * this.accelerationRate);

        this.currentVelocity = this.currentSpeed * this.transform.forward;
        this.carRigidBody.AddForce(this.currentVelocity, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.GetComponent<IBoostProvider>() is IBoostProvider boostProvider))
        {
            return;
        }
        if (Mathf.Approximately(this.remainingBoostTimeInSeconds, this.maxBoostTimeInSeconds))
        {
            return;
        }

        this.remainingBoostTimeInSeconds = Mathf.Clamp(this.remainingBoostTimeInSeconds + boostProvider.BoostProvided, 0, this.maxBoostTimeInSeconds);
        other.gameObject.SetActive(false);
    }

    private void ProcessInputs()
    {
        this.breakLightsMatProps.SetInt("_Emissive", Mathf.Approximately(0, inputs.decelerationInput) ? 0 : 1);
        this.bodyRenderer.SetPropertyBlock(breakLightsMatProps, 1);

        bool isBoostPermitted = inputs.boostInput > 0.5f && remainingBoostTimeInSeconds > 0f;
        float permittedBoostAmount = 0f;
        // if boost is requested & can be permitted
        if (isBoostPermitted)
        {
            permittedBoostAmount = inputs.boostInput * this.boostSpeedModifier;
            remainingBoostTimeInSeconds -= Time.deltaTime;
        }
        this.currentSpeedRequest = (this.maxSpeedWithoutBoost + permittedBoostAmount) * (inputs.accelerationRequest + (isBoostPermitted ? inputs.boostInput : 0));

        if (permittedBoostAmount > 0f && !boostParticleSystem.isPlaying)
        {
            boostParticleSystem.Play();
        }
        else if (permittedBoostAmount <= 0f && boostParticleSystem.isPlaying)
        {
            boostParticleSystem.Stop();
        }
         
        foreach (Animator propellorAnimator in this.propellorAnimators)
        {
            float oldPropellorSpeed = propellorAnimators[0].GetFloat("Speed"); 

            float newPropellorSpeed = Mathf.Lerp(oldPropellorSpeed, Mathf.Max(Mathf.Abs(this.currentSpeedRequest * 0.4f), Mathf.Abs(inputs.currentSteerRequest.y * steerRate.y * 2f)) * Time.deltaTime * 10, Time.deltaTime);
            propellorAnimator.SetFloat("Speed", newPropellorSpeed);
            propellorAudioSource.volume = newPropellorSpeed * 1f; 
        }
    }

    public void UpdatePropellorRotation(float acceleration, float turn)
    {
        float newYRot = acceleration >= 0f ? 90f * turn : 180f - (90f * turn);

        newYRot = newYRot < 0 ? 360 + newYRot : newYRot;

        foreach (Transform propellorArm in propellorArms)
        {
            // Use Mathf.LerpAngle to ensure shortest path interpolation for angles
            float y = Mathf.LerpAngle(propellorArm.localEulerAngles.y, newYRot, 2f * Time.deltaTime);
            propellorArm.localEulerAngles = new Vector3(0f, y, 0f);
        }
    }

}
