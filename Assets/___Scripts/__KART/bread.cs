using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public float speed = 40f;
    public float detectionRadius = 2f;
    private GameObject closestTarget;

    public List<GameObject> BOT;

    public GameObject parent_target;
    public List<GameObject> target;
    public static int[] target_value_obj = new int[8];
    private int target_value;

    private Tween detectionTween;
    private void Awake()
    {
        foreach (Transform child in parent_target.transform)
        {
            target.Add(child.gameObject);
        }
        target_value = 0;
    }

    private void OnEnable()
    {
        int max_value = Mathf.Max(target_value_obj);
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            if (target_value_obj[i] == max_value) count++;
        }
        target_value = target_value_obj[gameObject.name[7] - '0'];
        if (car_ctrl.rank == 1 && gameObject.name[7] == '7' && target_value_obj[7] == max_value)
        {
            target_value--;
            detectionRadius = 15;
        }
        else if (target_value_obj[gameObject.name[7] - '0'] == max_value && count == 1)
        {
            target_value--;
            detectionRadius = 15;
        }
        closestTarget = null;
        detectionTween = DOTween.To(() => detectionRadius, x => detectionRadius = x, 15, 1.5f);
        CancelInvoke("set_active_false");
        Invoke("set_active_false", 5f);
    }
    private void OnDisable()
    {
        detectionTween.Kill();
        detectionTween = null;
        detectionRadius = 2;
    }
    void set_active_false()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (closestTarget == null)
        {
            Find_BOT_Target();
        }

        if (closestTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, closestTarget.transform.position, speed * Time.deltaTime);
            transform.LookAt(closestTarget.transform);
        }
    }

    void Find_BOT_Target()
    {
        float shortestDistance = Mathf.Infinity;
        int i_min = -1;
        for (int i = 0; i < BOT.Count; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, BOT[i].transform.position);
            if (Vector3.Distance(transform.position, BOT[i].transform.position) < shortestDistance &&
                Vector3.Distance(transform.position, BOT[i].transform.position) < detectionRadius)
            {
                shortestDistance = distanceToEnemy;
                i_min = i;
            }
        }

        if (i_min != -1)
        {
            closestTarget = BOT[i_min];
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target[target_value].transform.position, speed * Time.deltaTime);
            transform.LookAt(target[target_value].transform);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BOT"))
        {
            if (other.transform.parent.parent.name[5] != gameObject.name[7])
            {
                gameObject.SetActive(false);
                Sound_Manager.Instance.Play_Sound("BreadHit");
            }
        }
        if (other.gameObject.CompareTag("target_point"))
        {
            int max_value = Mathf.Max(target_value_obj);
            target_value = other.gameObject.name[7] - '0';
            if (other.gameObject.name[8] != ')')
            {
                target_value *= 10;
                target_value += other.gameObject.name[8] - '0';
            }
            if (target_value_obj[gameObject.name[7] - '0'] == max_value)
            {
                if (target_value == 0)
                {
                    target_value = 59;// target point(max)
                }
                else
                {
                    target_value--;
                }
            }
            else
            {
                if (target_value == 59) // target point(max)
                {
                    target_value = 0;
                }
                else
                {
                    target_value++;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Chọn màu cho vòng tròn
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Vẽ một vòng tròn có bán kính là detectionRadius xung quanh nhân vật
    }

    //void OnDrawGizmos()
    //{
    //    if (target == null || target.Count < 2)
    //        return;

    //    Gizmos.color = Color.green; // Đặt màu cho Gizmos. Bạn có thể thay đổi màu này theo ý muốn.

    //    for (int i = 0; i < target.Count - 1; i++)
    //    {
    //        if (target[i] == null || target[i + 1] == null)
    //            continue;

    //        Gizmos.DrawLine(target[i].transform.position, target[i + 1].transform.position);
    //    }
    //}

}
