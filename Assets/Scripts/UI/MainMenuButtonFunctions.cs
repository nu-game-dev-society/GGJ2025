using UnityEngine;

public class MainMenuButtonFunctions : MonoBehaviour
{
    
    public void QuitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
