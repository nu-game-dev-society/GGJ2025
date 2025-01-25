using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class AiInputController : InputController
{
    public SplineContainer splineContainer;

    public float t = 0.0f;
    public float tStep = 1.0f;
    public float acceptanceV = 3.0f;

    public Rigidbody body;

    void Start()
    {
    }
    Vector3 pos;
    Vector3 target;
    Vector3 dir;
    public Vector3 crossProduct;

    public float pitchAngle;

    void Update()
    {
        pos = transform.position;
        target = splineContainer.EvaluatePosition(t);

        if (Vector3.Distance(pos, target) < acceptanceV)
        {
            t += tStep;
            if (t >= 1.0f)
                t -= 1.0f;
        }

        dir = (target - pos).normalized;

        currentSteerRequest = Vector3.zero;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        Quaternion currentRotation = transform.rotation;
        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(currentRotation);

        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 0.01f)
        {
            Vector3 torque = axis * angle * Mathf.Deg2Rad * 15f;
            body.AddTorque(torque);

            body.angularVelocity *= 1f;
        }


        accelerationRequest = 1f;

        //Debug.Log(currentSteerRequest);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 1.0f);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(pos, pos + (dir * acceptanceV));

    }
}
