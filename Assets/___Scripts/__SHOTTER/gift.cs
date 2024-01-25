using UnityEngine;

public class gift : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rb.useGravity = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
}
