using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float startTime = 60f;
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string timeUpSceneName = "Timeup";

    private float timeRemaining;
    private bool isCountingDown = true;
    private bool hasTriggeredTimeUp = false;

    void Start()
    {
        timeRemaining = startTime;
        UpdateDisplay();
    }

    void Update()
    {
        if (!isCountingDown || hasTriggeredTimeUp)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isCountingDown = false;
                TriggerTimeUp();
            }
            UpdateDisplay();
        }
    }

    private void TriggerTimeUp()
    {
        hasTriggeredTimeUp = true;
        DestroyPlayer();
        SceneManager.LoadScene(timeUpSceneName, LoadSceneMode.Additive);
    }

    private void DestroyPlayer()
    {
        GameObject player = GameObject.FindWithTag(playerTag);
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            Debug.LogWarning($"TimeManager: No GameObject found with tag '{playerTag}' to destroy.");
        }
    }

    private void UpdateDisplay()
    {
        if (timeDisplay != null)
        {
            timeDisplay.text = $"Time: {timeRemaining:F0}";
        }
    }
}
