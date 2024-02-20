using UnityEngine;

public class camera_glass_bridge : MonoBehaviour
{
    public GameObject player;
    private Vector3 khoang_cach;
    public static bool old_next = false;
    private void Awake()
    {
        Time.timeScale = 1;
        khoang_cach = (transform.position - player.transform.position);
    }
    private void FixedUpdate()
    {
        Vector3 new_position = player.transform.position + khoang_cach;
        //if (player_glass_bridge.next)
        //{
        //    new_position = player.transform.position + khoang_cach + new Vector3(10, 0, 0);
        //    old_next = player_glass_bridge.next;
        //}
        //if (!player_glass_bridge.next && old_next)
        //{
        //    transform.position = player.transform.position + khoang_cach + new Vector3(-15, 0, 0);
        //    old_next = player_glass_bridge.next;
        //}
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 3 * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
