using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AiInputController : InputController
{
    public SplineContainer splineContainer;

    public float t = 0.0f;
    public float tStep = 1.0f;
    public float acceptanceV = 3.0f;


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
        }

        dir = (target - pos).normalized;


        currentSteerRequest = Vector3.zero;

        //yaw
        crossProduct = Vector3.Cross(dir, transform.forward);
        float angle = Vector3.Angle(dir, transform.forward);

        if (crossProduct.y > 0)
        {
            currentSteerRequest.y = -angle; 
        }
        else
        {
            currentSteerRequest.y = angle;  
        }
        // Pitch (Vertical Rotation)
        float targetHeight = target.y - pos.y; // Get vertical difference between target and turret
        float targetDistance = new Vector3(dir.x, 0, dir.z).magnitude; // Horizontal distance between turret and target
        pitchAngle = Mathf.Atan2(targetHeight, targetDistance) * Mathf.Rad2Deg; // Calculate pitch angle in degrees

        currentSteerRequest.x = -pitchAngle;


        currentSteerRequest.x = Mathf.Clamp(currentSteerRequest.x, -1.0f, 1.0f);//pitch
        currentSteerRequest.y = Mathf.Clamp(currentSteerRequest.y, -1.0f, 1.0f);//yaw


        accelerationRequest = 1.0f;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 1.0f);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(pos, pos + (dir * acceptanceV));

    }
}
