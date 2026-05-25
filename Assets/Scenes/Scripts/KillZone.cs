using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZone : MonoBehaviour
{
    [SerializeField]
    private bool useTrigger = true;

    [Header("Visibility")]
    [SerializeField]
    private bool hideRenderersOnStart = true;

    [HideInInspector]
    public bool isHidden = false;

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = useTrigger;
        }

        if (hideRenderersOnStart)
        {
            HideRenderers();
        }
    }

    public void HideRenderers()
    {
        SetRenderersVisible(false);
        isHidden = true;
    }

    public void ShowRenderers()
    {
        SetRenderersVisible(true);
        isHidden = false;
    }

    private void SetRenderersVisible(bool visible)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Player") || other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Player"))
        {
            Destroy(collision.collider.gameObject);
        }
    }
}

