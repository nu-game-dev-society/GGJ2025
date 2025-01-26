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

    private void OnDisable()
    {
        this.currentSteerRequest = Vector3.zero;
        this.decelerationInput = 0;
        this.boostInput = 0;
        this.accelerationRequest = 0;
        this.isHandbrakeOn = true;
    }

    private void OnEnable()
    {
        this.isHandbrakeOn = false;
    }

}
