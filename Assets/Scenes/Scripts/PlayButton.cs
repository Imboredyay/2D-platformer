using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private void OnMouseEnter()
    {
        SoundManager.Instance?.PlayHover();
    }

    private void OnMouseDown()
    {
        SoundManager.Instance?.PlayClick();
        SceneManager.LoadScene( "LevelAOne");


    }
}
