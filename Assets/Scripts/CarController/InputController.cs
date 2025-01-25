using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Vector3 currentSteerRequest;
    public float decelerationInput;
    public float boostInput;
    public float accelerationRequest;
    public bool isHandbrakeOn;
}
