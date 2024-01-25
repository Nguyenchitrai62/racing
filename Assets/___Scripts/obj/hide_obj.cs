using UnityEngine;

public class hide_obj : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            GetComponent<Collider>().isTrigger = true;
            GameObject.Find("player").GetComponent<Outline>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<Collider>().isTrigger = false;
            GameObject.Find("player").GetComponent<Outline>().enabled = false;
        }
    }
}
