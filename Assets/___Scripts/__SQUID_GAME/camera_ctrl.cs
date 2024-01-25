using DG.Tweening;
using UnityEngine;

public class camera_ctrl : MonoBehaviour
{
    public GameObject player;
    private Vector3 khoang_cach;
    private void Awake()
    {
        khoang_cach = (transform.position - player.transform.position + new Vector3(0, 2, 0));
    }
    private void Start()
    {
        transform.DOMoveY(8, 2f).SetEase(Ease.Linear);

        transform.DORotate(new Vector3(20, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 2f).SetEase(Ease.Linear);
    }
    private void FixedUpdate()
    {
        if (player_ctrl.is_alive)
        {
            Vector3 new_position = player.transform.position + khoang_cach;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 3 * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
