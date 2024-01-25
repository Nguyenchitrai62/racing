using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    TextMeshProUGUI money_value;

    public static float cur_value;
    float start_time;
    private float cur_scale;
    public float target_scale = 1.3f;
    public static bool check = false;

    private void Awake()
    {
        cur_scale = transform.localScale.x;
        money_value = GetComponentInChildren<TextMeshProUGUI>();
        cur_value = canvas_controller.money;
    }

    private void OnEnable()
    {
        StartCoroutine(UnscaledLateUpdate());
    }

    IEnumerator UnscaledLateUpdate()
    {
        while (gameObject.activeSelf)
        {
            if (Time.unscaledTime - start_time > 0.5f)
            {
                money_value.color = Color.white;
                set_cur_value();
            }
            if (cur_value != canvas_controller.money)
            {
                ScaleMoneyEffect();

                if (check && canvas_controller.money > 0) Sound_Manager.Instance.Play_Sound("CashOut");

                if (cur_value > canvas_controller.money)
                {
                    money_value.color = Color.red;
                    start_time = Time.unscaledTime;
                }
                if (cur_value < canvas_controller.money)
                {
                    money_value.color = Color.green;
                    start_time = Time.unscaledTime;
                }
                cur_value = canvas_controller.money;
                check = true;
            }
            yield return null; // Đợi cho tới frame tiếp theo
        }
    }

    public void set_cur_value()
    {
        if (draw.check && canvas_controller.money == 0)
        {
            cur_value = 1;
        }
    }

    void ScaleMoneyEffect()
    {
        Sequence s = DOTween.Sequence();

        s.Append(transform.DOScale(target_scale, 0.2f).SetUpdate(true));
        s.Append(transform.DOScale(cur_scale, 0.2f).SetUpdate(true));

        s.Play();
    }
}
