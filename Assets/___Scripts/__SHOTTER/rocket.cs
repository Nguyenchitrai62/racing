using UnityEngine;

public class rocket : MonoBehaviour
{
    public ParticleSystem no;
    public ParticleSystem chay;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        no.transform.position = transform.position;
        chay.transform.position = transform.position;
        no.gameObject.SetActive(true);
        chay.gameObject.SetActive(true);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        transform.position = transform.parent.transform.position;
    }
}
