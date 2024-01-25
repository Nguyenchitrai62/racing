using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Button_1 : MonoBehaviour
{
    public List<GameObject> door;
    public List<Vector3> vector;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            for (int i = 0; i < door.Count; i++)
            {
                door[i].transform.DOLocalMove(door[i].transform.localPosition + vector[i], 1).SetEase(Ease.Linear);
            }
        }
    }
}
