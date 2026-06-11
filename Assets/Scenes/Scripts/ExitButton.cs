using UnityEngine;

public class ExitButton : MonoBehaviour
{
    private void OnMouseEnter()
    {
        SoundManager.Instance?.PlayHover();
    }

    private void OnMouseDown()
    {
        SoundManager.Instance?.PlayClick();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
