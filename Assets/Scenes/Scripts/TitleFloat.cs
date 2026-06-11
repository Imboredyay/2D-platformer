using UnityEngine;

public class TitleFloat : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPosition + Vector3.up * offset;
    }
}
