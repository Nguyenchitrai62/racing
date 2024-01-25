using UnityEngine;

public class bread_BOT : MonoBehaviour
{
    public GameObject player;
    public float speed = 40;
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.LookAt(player.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            gameObject.SetActive(false);
        }
    }
}
