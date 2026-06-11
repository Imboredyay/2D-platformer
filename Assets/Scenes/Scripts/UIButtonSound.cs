using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public bool playHover = true;
    public bool playClick = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playHover)
        {
            SoundManager.Instance?.PlayHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playClick)
        {
            SoundManager.Instance?.PlayClick();
        }
    }
}
