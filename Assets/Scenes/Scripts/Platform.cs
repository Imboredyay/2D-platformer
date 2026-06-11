using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
    [Header("Targets")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 2f;
    [Tooltip("If true the saw will move back and forth between points. If false it will loop A->B->A.")]
    public bool pingPong = true;
    [Tooltip("Seconds to wait when reaching each point.")]
    public float waitAtPoint = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 180f;

    Transform currentTarget;
    float waitTimer;
    Transform playerTransform;

    void Reset()
    {
        // Provide sensible defaults when added to an object in the editor
        if (pointA == null) pointA = transform;
        if (pointB == null) pointB = transform.parent != null ? transform.parent : transform;
    }

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("SawController: assign both pointA and pointB for movement. Movement will be disabled.");
            currentTarget = null;
            return;
        }

        currentTarget = pointB;
        waitTimer = 0f;
    }

    void Update()
    {
        // Always rotate the saw
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (currentTarget == null)
            return;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        Vector3 pos = transform.position;
        Vector3 targetPos = currentTarget.position;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(pos, targetPos, step);

        if (Vector3.Distance(transform.position, targetPos) <= 0.01f)
        {
            waitTimer = waitAtPoint;
            // swap targets
            if (currentTarget == pointA)
                currentTarget = pointB;
            else
                currentTarget = pointA;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (pointA != null) Gizmos.DrawSphere(pointA.position, 0.1f);
        if (pointB != null) Gizmos.DrawSphere(pointB.position, 0.1f);
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Player") || other.CompareTag("player"))
        {
            playerTransform = other.transform;
            playerTransform.SetParent(transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Player") || other.CompareTag("player"))
        {
            if (playerTransform == other.transform)
            {
                playerTransform.SetParent(null);
                playerTransform = null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("player"))
        {
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("player"))
        {
            if (playerTransform == collision.transform)
            {
                playerTransform.SetParent(null);
                playerTransform = null;
            }
        }
    }
}
