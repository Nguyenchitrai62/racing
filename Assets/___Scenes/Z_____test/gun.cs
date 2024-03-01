using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public GameObject bullet;
    public GameObject obj_tarrget;

    void Update()
    {
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * 5;
        if (Vector2.Distance(bullet.transform.position, transform.position) >= 5) bullet.transform.position = transform.position;
        
        if (obj_tarrget != null)
        {
            Vector3 target = obj_tarrget.transform.position - transform.position;
            float angle = Vector2.Angle(target,Vector2.up);
            if(target.x > 0) angle = -angle;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (obj_tarrget == null)
        {
            obj_tarrget = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == obj_tarrget) obj_tarrget = null;
    }
}
