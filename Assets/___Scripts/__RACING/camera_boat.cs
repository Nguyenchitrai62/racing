using UnityEngine;

public class camera_boat : MonoBehaviour
{
    public GameObject player;
    private Vector3 khoang_cach;

    private void Awake()
    {
        khoang_cach = (transform.position - player.transform.position);
    }

    private void LateUpdate()
    {

        Vector3 new_position = player.transform.position + khoang_cach;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 5 * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
