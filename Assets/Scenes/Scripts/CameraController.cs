using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);
    public float smoothTime = 0.15f;
    public bool followX = true;
    public bool followY = true;
    public bool forwardXOnly = false;

    private Vector3 currentVelocity;
    private float minY;

    void Start()
    {
        minY = transform.position.y;

        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
        }

        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;
        Vector3 desiredPosition = transform.position;

        if (followX)
        {
            float nextX = targetPosition.x;
            if (forwardXOnly)
            {
                nextX = Mathf.Max(transform.position.x, targetPosition.x);
            }
            desiredPosition.x = nextX;
        }

        if (followY)
            desiredPosition.y = targetPosition.y;

        desiredPosition.y = Mathf.Max(desiredPosition.y, minY);
        desiredPosition.z = offset.z;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
    }
}
