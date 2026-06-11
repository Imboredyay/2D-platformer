using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("UI Sounds")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float uiVolume = 1f;

    [SerializeField]
    private AudioSource uiSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (uiSource == null)
        {
            uiSource = gameObject.AddComponent<AudioSource>();
        }

        uiSource.playOnAwake = false;
    }

    public void PlayHover()
    {
        PlaySound(hoverSound);
    }

    public void PlayClick()
    {
        PlaySound(clickSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || uiSource == null)
        {
            return;
        }

        uiSource.volume = uiVolume;
        uiSource.PlayOneShot(clip);
    }
}
