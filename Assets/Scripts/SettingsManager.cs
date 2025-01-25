using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool IsPlayerPitchInputInverted { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception("How did we get 2 settings managers?");
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}