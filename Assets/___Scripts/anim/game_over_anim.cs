using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class game_over_anim : MonoBehaviour
{
    public TextMeshProUGUI score_value_text;
    public GameObject High_score;
    public List<GameObject> active_game_object;
    public List<GameObject> confetti;

    float score_value_max = 0;
    float i = 0;
    private void OnEnable()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        i = 0;
        score_value_text.text = "0";
        score_value_max = PlayerPrefs.GetFloat("score" + gameObject.scene.name, 0);
        if (score_value_max > PlayerPrefs.GetFloat("high_score" + gameObject.scene.name, 0))
        {
            Invoke("high_score_anim", 1.5f);
            Invoke("play_confetti_anim", 2f);
        }
        score_anim();
        Invoke("active", 2);
    }
    void score_anim()
    {
        if (i < 40) 
        {
            score_value_text.text = Mathf.Round(score_value_max * i / 40f).ToString();
            i++;
            Invoke("score_anim", 0.05f);
        }
        else
        {
            score_value_text.text = score_value_max.ToString();
        }
    }
    void active()
    {
        for (int i = 0; i < active_game_object.Count; i++)
        {
            active_game_object[i].SetActive(true);
        }
    }
    void high_score_anim()
    {
        Sound_Manager.Instance.Play_Sound("Confetti");
        for (int i = 0; i < 2; i++)
        {
            confetti[i].SetActive(true);
        }

        PlayerPrefs.SetFloat("high_score" + gameObject.scene.name, score_value_max);
        High_score.SetActive(true);
        PlayScaleEffect();
    }

    void play_confetti_anim()
    {
        Sound_Manager.Instance.Play_Sound("Confetti");
        for (int i = 2; i < 4; i++)
        {
            confetti[i].SetActive(true);
        }
    }
    void PlayScaleEffect()
    {
        // Tạo hiệu ứng scale cho biểu tượng high score
        High_score.transform.localScale = new Vector3(10,10,10); // Đặt kích thước ban đầu
        High_score.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f)
            .SetEase(Ease.OutQuad);
            //.SetLoops(2, LoopType.Yoyo);
    }
}
