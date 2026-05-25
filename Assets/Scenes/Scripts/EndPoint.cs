using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class EndPoint : MonoBehaviour
{
    [SerializeField] private string nextLevelName = "Level2";
    
    [SerializeField] private bool isColliderTrigger = true;

    void Start()
    {
        // Optionally ensure the collider is set as a trigger if this is the endpoint
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && isColliderTrigger)
        {
            collider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Player") || other.CompareTag("player"))
        {
            LoadNextLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("player"))
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelName))
        {
            Debug.LogError("EndPoint: Next level name is not set!");
            return;
        }

        Debug.Log($"Loading level: {nextLevelName}");
        SceneManager.LoadScene(nextLevelName);
    }

    void OnDrawGizmosSelected()
    {
        // Draw endpoint marker in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
