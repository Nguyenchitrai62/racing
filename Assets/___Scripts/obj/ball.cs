using UnityEngine;

public class ball : MonoBehaviour
{
    public float speed;
    private void OnEnable()
    {
        speed = 10;
        if (canvas_controller.range > 10)
        {
            speed = 15;
        }
    }
    void Update()
    {
        if (Circle.check)
        {
            transform.position = Vector3.MoveTowards(transform.position, Circle.target_object, speed * Time.deltaTime);
        }
        if (transform.position == Circle.target_object)
        {
            gameObject.SetActive(false);
            Circle.check = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") && Circle.check)
        {
            gameObject.SetActive(false);
            Circle.check = false;
        }
    }
}
