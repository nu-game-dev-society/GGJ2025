using UnityEngine;

public class InputController : MonoBehaviour
{
    [field: SerializeField]
    public string PlayerName { get; set; }

    public Vector3 currentSteerRequest;
    public float decelerationInput;
    public float boostInput;
    public float accelerationRequest;
    public bool isHandbrakeOn;

}
