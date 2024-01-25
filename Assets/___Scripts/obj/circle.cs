using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject ball;
    public static Vector3 target_object;
    public static bool check;

    float rotate_temp;
    private void Awake()
    {
        check = false;
        rotate_temp = transform.parent.eulerAngles.z;
    }
    private void Update()
    {
        transform.eulerAngles = new Vector3(90, 0, rotate_temp);
        rotate_temp += 0.5f;
        transform.Rotate(0, 0, 40 * Time.deltaTime);
        transform.localScale = new Vector3(0.1f + 0.02f * canvas_controller.range, 0.1f + 0.02f * canvas_controller.range, 0.1f + 0.02f * canvas_controller.range);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy")/* || other.CompareTag("destroy")*/)
        {
            Sound_Manager.Instance.Play_Sound("Throw");
            target_object = other.transform.position + new Vector3(0, 2, 0);
            ball.transform.SetParent(null);
            gameObject.SetActive(false);
            check = true;
        }
    }
}
