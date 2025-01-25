using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    [SerializeField]
    private Rigidbody PlayerController;

    private Vector3 StartPos;
    public Vector3 Range;
    public Vector3 Magnitude;


    void Start()
    {
        StartPos = transform.localPosition;
    }


    void FixedUpdate()
    {
        Vector3 localVelThisFrame = PlayerController.transform.InverseTransformDirection(PlayerController.velocity);

        Vector3 clamped = new Vector3(
            Mathf.Sign(localVelThisFrame.x) * Mathf.Lerp(0, Range.x, (Mathf.Abs(localVelThisFrame.x) / 20f)),
             Mathf.Sign(localVelThisFrame.y) * Mathf.Lerp(0, Range.y, (Mathf.Abs(localVelThisFrame.y) / 20f)),
             Mathf.Sign(localVelThisFrame.z) * Mathf.Lerp(0, Range.z, (Mathf.Abs(localVelThisFrame.z) / 20f)));

        transform.localPosition = StartPos + clamped;
    }
}
