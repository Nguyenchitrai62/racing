using UnityEngine;

public class destroy : MonoBehaviour
{
    public float speed = 4f; // Tốc độ di chuyển của object
    public float height = 0.2f; // Độ cao mà object có thể bay lên
    private Vector3 startPosition; // Vị trí ban đầu của object

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * height + startPosition.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            gameObject.SetActive(false);
        }
    }
}
