using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    [SerializeField]
    private Rigidbody PlayerController;

    private Vector3 StartPos;
    public Vector3 Range;
    public float Magnitude = 0.1f;

    private Vector3 prevVel;
    void Start()
    {
        StartPos = transform.localPosition;

        prevVel = PlayerController.transform.InverseTransformDirection(PlayerController.velocity);
    }


    void Update()
    {
        Vector3 localVelThisFrame = PlayerController.transform.InverseTransformDirection(PlayerController.velocity);

        Vector3 velocityChange = prevVel - localVelThisFrame;
        prevVel = localVelThisFrame;

        Vector3 clamped = new Vector3(
            Mathf.Clamp(velocityChange.x, -Range.x, Range.x),
            Mathf.Clamp(velocityChange.y, -Range.y, Range.y),
            Mathf.Clamp(velocityChange.z, -Range.z, Range.z));
        transform.localPosition = Magnitude * clamped;
    }
}
