using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip gameOverClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (backgroundSource == null)
                backgroundSource = GetComponent<AudioSource>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayGameOver()
    {
        if (backgroundSource != null)
        {
            backgroundSource.Stop();
            if (gameOverClip != null)
            {
                backgroundSource.clip = gameOverClip;
                backgroundSource.loop = false;
                backgroundSource.Play();
                return;
            }
        }

        if (sfxSource != null && gameOverClip != null)
        {
            sfxSource.PlayOneShot(gameOverClip);
            return;
        }

        Debug.LogWarning("AudioManager: No audio source or gameOverClip assigned to play GameOver music.");
    }
}
