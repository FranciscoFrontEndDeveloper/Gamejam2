using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CollectManagerr>().OnCoinCollected();
            Destroy(gameObject);
        }
    }
}