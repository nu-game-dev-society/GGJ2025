using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool IsPlayerPitchInputInverted { get; set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}