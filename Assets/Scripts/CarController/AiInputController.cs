using System;
using UnityEngine;
using UnityEngine.Splines;

public class AiInputController : InputController
{
    public SplineContainer splineContainer;

    public float lookAheadPercentage = 0.1f; // Percentage of the spline to look ahead
    public float acceptanceV = 3.0f;
    public float turnSpeed = 15f;

    public Rigidbody body;
    public bool disablename;

    private float t = 0.0f; // Current percentage along the spline
    private Vector3 pos;    // Current position of the AI
    private Vector3 target; // Target position on the spline
    private Vector3 dir;    // Direction towards the target
    private Vector3 offset; // Random offset for each car

    void Awake()
    {
        if (!disablename)
        {
            string v;
            do
            {
                v = DriverNames.GetRandom();
            } while (GameObject.Find(v) != null && v != DriverNames.GetPlayerDriverName());
            PlayerName = v;
        }

        currentSteerRequest = Vector3.zero;
        boostInput = 1.0f;

        // Generate a random offset for this car
        // X offset for sideways, Y offset for vertical adjustments
        float offsetX = UnityEngine.Random.Range(-2.0f, 2.0f); // Slight side offset
        float offsetY = UnityEngine.Random.Range(-2f, 2f); // Slight height offset
        offset = new Vector3(offsetX, offsetY, 0);
    }

    void Update()
    {
        pos = transform.position;

        // Get the nearest point on the spline and update the current t value
        SplineUtility.GetNearestPoint(splineContainer.Spline, pos, out _, out t);

        // Calculate lookahead t value and wrap around if necessary
        float lookAheadT = t + lookAheadPercentage;
        if (lookAheadT >= 1.0f)
            lookAheadT -= 1.0f;

        // Calculate the target position ahead on the spline
        target = splineContainer.EvaluatePosition(lookAheadT);

        // Calculate the spline's tangent and normal for offset application
        Vector3 tangent = splineContainer.EvaluateTangent(lookAheadT);
        tangent = tangent.normalized;

        Vector3 normal = Vector3.Cross(tangent, Vector3.up).normalized;

        // Apply the offset relative to the spline's orientation
        target += normal * offset.x; // Sideways offset
        target += Vector3.up * offset.y; // Vertical offset

        Vector3 targetLocal = transform.InverseTransformPoint(target);

        // Calculate direction towards the target
        dir = targetLocal;

        // Reset steer request
        currentSteerRequest = Vector3.zero;

        // Map the local axis to steer requests 
        currentSteerRequest.x = dir.y / 90f * -turnSpeed;
        currentSteerRequest.y = dir.x / 90f * turnSpeed;

        // Set constant acceleration 
        accelerationRequest = 1f - (Mathf.Abs(currentSteerRequest.x + currentSteerRequest.y) / 2f);
    }

    private void OnDrawGizmos()
    {
        // Visualize the target position and direction
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 1.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + (dir * acceptanceV));

        // Visualize the offset point
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target - offset, 0.5f);
    }
}
