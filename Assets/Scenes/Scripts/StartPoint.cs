using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    
    [Header("Spawn Offset")]
    [SerializeField] private Vector2 spawnOffset = Vector2.zero;
    
    [Tooltip("If true, spawn offset is applied relative to this object's local axes.")]
    [SerializeField] private bool useLocalOffset = false;

    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("StartPoint: Player prefab not assigned! Assign it in the Inspector.");
            return;
        }

        // Calculate spawn position
        Vector3 spawnPosition;
        
        if (useLocalOffset)
        {
            spawnPosition = transform.position + transform.TransformDirection(spawnOffset);
        }
        else
        {
            spawnPosition = transform.position + (Vector3)spawnOffset;
        }

        // Instantiate player at spawn position
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        
        Debug.Log($"Player spawned at {spawnPosition}");
    }

    void OnDrawGizmosSelected()
    {
        // Draw spawn point in the editor for visualization
        Gizmos.color = Color.green;
        Vector3 spawnPosition = transform.position + (Vector3)spawnOffset;
        Gizmos.DrawSphere(spawnPosition, 0.2f);
        
        // Draw line from StartPoint to spawn position
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, spawnPosition);
    }
}
