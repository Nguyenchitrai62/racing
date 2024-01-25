using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public GameObject player;
    private Vector3 khoang_cach;
    private void Reset()
    {
#if UNITY_EDITOR
        Load_Component();
#endif
    }
    void Load_Component()
    {
        player = GameObject.Find("player").gameObject;
    }

    private void Awake()
    {
        khoang_cach = (transform.position - player.transform.position);
    }

    private void LateUpdate()
    {
        if (canvas_controller.Instance.play)
        {
            Vector3 new_position = player.transform.position + khoang_cach;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 3 * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        if (canvas_controller.Instance.main_menu)
        {
            Vector3 new_position = player.transform.position + khoang_cach * 2 - new Vector3(0, 0, 2);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 4 * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        if (canvas_controller.Instance.change_skin)
        {
            transform.position = new Vector3(0, 100, 0);
        }
    }
}
