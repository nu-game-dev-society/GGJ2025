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
    public Vector3 fwdCross;

    public string statusFwd;
    public string statusPtch;

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

        fwdCross = Vector3.Cross(transform.forward, dir);
        Vector3 rightCross = Vector3.Cross(transform.right, dir);
        Vector3 upCross = Vector3.Cross(transform.up, dir);



        if (fwdCross.y > 0.0f && rightCross.y > 0.0f)
        {
            //BL
            fwdCross.y = -1.0f;
            statusFwd = "BR";
        }
        else if (fwdCross.y < 0.0f && rightCross.y > 0.0f)
        {
            //BR flip
            fwdCross.y = 1.0f;
            statusFwd = "BL";
        }
        else
        {
            statusFwd = string.Empty;
        }

        if (fwdCross.x > 0.0f && upCross.x > 0.0f)
        {
            fwdCross.x = 1.0f;
            statusPtch = "BR";
        }
        else if (fwdCross.x < 0.0f && upCross.x > 0.0f)
        {
            //BR flip
            fwdCross.x = -1.0f;
            statusPtch = "BL";
        }
        else
        {
            statusPtch = string.Empty;
        }
        currentSteerRequest.y = fwdCross.y;
        currentSteerRequest.x = fwdCross.x;

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