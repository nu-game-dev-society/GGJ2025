using UnityEngine;

public class InputController : MonoBehaviour
{
    public string PlayerName { get => playerName; set { gameObject.name = value; playerName = value; } }
    private string playerName = string.Empty;
    public Vector3 currentSteerRequest;
    public float decelerationInput;
    public float boostInput;
    public float accelerationRequest;
    public bool isHandbrakeOn;

}
