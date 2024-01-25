using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class BOT_main_game : MonoBehaviour
{
    public float speed;
    private Vector3 targetPosition;

    Rigidbody rb;
    Animator anim;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        SetNewRandomTarget();
    }
    private void FixedUpdate()
    {
        Debug.Log(timer);
        if (Vector3.Distance(targetPosition, transform.position) < 0.1f)
        {
            anim.SetInteger("state", 0);
        }
        else anim.SetInteger("state", 1);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetNewRandomTarget();
            timer = 4f;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, -30f, 0) * transform.forward, out hit, 2f))
        {
            targetPosition = transform.position;
            timer = 0;
            transform.Rotate(Vector3.up, 3);
        }
        else
        if (Physics.Raycast(transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, 30f, 0) * transform.forward, out hit, 2f))
        {
            targetPosition = transform.position;
            timer = 0;
            transform.Rotate(Vector3.up, -3);
        }




        Vector3 huong = targetPosition - transform.position;
        huong.Normalize();
        rb.velocity = huong * speed;








        Debug.DrawRay(transform.position + new Vector3(0f, 0.25f, 0), (Quaternion.Euler(0f, -30, 0f) * transform.forward) * 2f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0f, 0.25f, 0), (Quaternion.Euler(0f, 30, 0f) * transform.forward) * 2f, Color.red);
    }
    void SetNewRandomTarget()
    {
        float randomX;
        float randomZ;
        if (Random.value < 0.5f) randomX = Random.Range(-10f, -5f);
        else randomX = Random.Range(5f, 10f);
        if (Random.value < 0.5f) randomZ = Random.Range(-10f, -5f);
        else randomZ = Random.Range(5f, 10f);

        targetPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        Quaternion lookOnLook = Quaternion.LookRotation(targetPosition - transform.position);
        transform.DORotateQuaternion(lookOnLook, 1f).SetEase(Ease.InOutSine);
    }
}
