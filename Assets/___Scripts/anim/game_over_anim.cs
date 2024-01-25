using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class game_over_anim : MonoBehaviour
{
    public TextMeshProUGUI score_value_text;

    float score_value = 0;
    float score_value_max = 0;
    private void OnEnable()
    {
        score_value = 0;
        score_value_text.text = score_value.ToString();
        score_value_max = PlayerPrefs.GetFloat("score_mini_game");
        score_anim();
    }
    void score_anim()
    {
        if (score_value + Mathf.Round(score_value_max / 40) < PlayerPrefs.GetFloat("score_mini_game")) 
        {
            score_value += Mathf.Round(score_value_max / 40);
            score_value_text.text = score_value.ToString();
            Invoke("score_anim", 0.05f);
        }
        else
        {
            score_value = score_value_max;
            score_value_text.text = score_value.ToString();
        }
    }
}
