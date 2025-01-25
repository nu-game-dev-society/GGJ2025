using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AitestScript : MonoBehaviour
{
    public Transform target;

    void Start()
    {
    }
    public Vector3 dir;
    public Vector3 fwdCross;
    public Vector3 rightCross;
    public Vector3 upCross;


    public Vector3 currentSteerRequest;

    public bool backwards;
    public string statusFwd;
    public string statusPtch;

    void Update()
    {

        dir = (target.position - transform.position).normalized;


        currentSteerRequest = Vector3.zero;

        fwdCross = Vector3.Cross(transform.forward, dir);
        rightCross = Vector3.Cross(transform.right, dir);
        upCross = Vector3.Cross(transform.up, dir);

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
            fwdCross.x = -1.0f;
            statusPtch = "BR";
        }
        else if (fwdCross.x < 0.0f && upCross.x > 0.0f)
        {
            //BR flip
            fwdCross.x = 1.0f;
            statusPtch = "BL";
        }
        else
        {
            statusPtch = string.Empty;
        }

        currentSteerRequest.y = fwdCross.x;
        currentSteerRequest.x = fwdCross.y;

        currentSteerRequest.x = Mathf.Clamp(currentSteerRequest.x, -1.0f, 1.0f);//pitch
        currentSteerRequest.y = Mathf.Clamp(currentSteerRequest.y, -1.0f, 1.0f);//yaw

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 1.0f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1.0f);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position, transform.position + (dir * 5.0f));

    }

}
