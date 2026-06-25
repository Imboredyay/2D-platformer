using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SafeSawController : MonoBehaviour
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

    void Reset()
    {
        if (pointA == null) pointA = transform;
        if (pointB == null) pointB = transform.parent != null ? transform.parent : transform;
    }

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("SafeSawController: assign both pointA and pointB for movement. Movement will be disabled.");
            currentTarget = null;
            return;
        }

        currentTarget = pointB;
        waitTimer = 0f;
    }

    void Update()
    {
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
            if (currentTarget == pointA)
                currentTarget = pointB;
            else
                currentTarget = pointA;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pointA != null) Gizmos.DrawSphere(pointA.position, 0.1f);
        if (pointB != null) Gizmos.DrawSphere(pointB.position, 0.1f);
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
