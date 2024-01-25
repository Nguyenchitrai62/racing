using DG.Tweening;
using TMPro;
using UnityEngine;

public class score_whack_mouse : MonoBehaviour
{
    TextMeshProUGUI money_value;

    float cur_value;
    float start_time;
    private void Awake()
    {
        money_value = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        cur_value = mouse_minigame.score_mini_game;
    }
    private void LateUpdate()
    {
        if (Time.time - start_time > 0.5f)
        {
            money_value.color = Color.white;
        }
        if (cur_value != mouse_minigame.score_mini_game)
        {
            ScaleMoneyEffect();

            if (cur_value > mouse_minigame.score_mini_game)
            {
                money_value.color = Color.red;
                start_time = Time.time;
            }
            if (cur_value < mouse_minigame.score_mini_game)
            {
                money_value.color = Color.green;
                start_time = Time.time;
            }
            cur_value = mouse_minigame.score_mini_game;
        }

    }

    void ScaleMoneyEffect()
    {
        Sequence s = DOTween.Sequence();

        s.Append(transform.DOScale(0.3f, 0.2f));

        s.Append(transform.DOScale(0.25f, 0.2f));

        s.Play();

    }
}
