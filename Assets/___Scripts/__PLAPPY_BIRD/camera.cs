using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject _player;
    private Vector3 _khoang_cach;

    private void Awake()
    {
        _khoang_cach = (transform.position - _player.transform.position);
    }

    private void Update()
    {

        Vector3 new_position = _player.transform.position + _khoang_cach;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new_position, 3 * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
