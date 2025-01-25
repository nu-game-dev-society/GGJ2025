using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Aesthetics")]
    [SerializeField]
    private Material brakeLightsMaterial;

    [SerializeField]
    private Animator[] propellorAnimators;

    [SerializeField]
    private Transform[] propellorArms;

    [SerializeField]
    private ParticleSystem exhaustParticleSystem;

    [SerializeField]
    private float exhaustParticleSystemEmissionRateOverTimeBoostMultiplier;
    private float exhaustParticleSystemEmissionRateOverTimeWhenIdle;

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
        this.exhaustParticleSystemEmissionRateOverTimeWhenIdle = this.exhaustParticleSystem.emission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        this.ProcessInputs(); 

        UpdatePropellorRotation(inputs.accelerationRequest, inputs.currentSteerRequest.y);
    }

    private void FixedUpdate()
    {
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

    private void ProcessInputs()
    {
        this.brakeLightsMaterial.SetInt("_Emissive", Mathf.Approximately(0, inputs.decelerationInput) ? 0 : 1);

        float boostSpeed = inputs.boostInput * this.boostSpeedModifier;
        var emissionModule = this.exhaustParticleSystem.emission;
        emissionModule.rateOverTime = Mathf.Approximately(0, boostSpeed)
            ? this.exhaustParticleSystemEmissionRateOverTimeWhenIdle
            : this.exhaustParticleSystemEmissionRateOverTimeWhenIdle * this.exhaustParticleSystemEmissionRateOverTimeBoostMultiplier;
        Debug.Log(emissionModule.rateOverTime);

        float maxSpeed = this.maxSpeedWithoutBoost + boostSpeed;

        this.currentSpeedRequest = maxSpeed * inputs.accelerationRequest;
         
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
