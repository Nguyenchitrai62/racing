using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class hammer_ctrl : MonoBehaviour
{
    public GameObject hammer;
    private RectTransform hammer_rect;
    public GameObject end_minigame;
    Tween tween;

    public GameObject HIT_prefab;
    private List<RectTransform> HIT_pool = new List<RectTransform>();

    private GameObject parent;
    private int i = 0;
    private void Awake()
    {
        hammer_rect = hammer.GetComponent<RectTransform>();
        parent = GameObject.Find("Canvas").gameObject;
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(HIT_prefab);
            obj.transform.localScale = new Vector3(Screen.width / 10, Screen.width / 10, Screen.width / 10);
            HIT_pool.Add(obj.GetComponent<RectTransform>());
            obj.SetActive(false);
            obj.transform.SetParent(parent.transform);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 1 && !end_minigame.activeSelf)
        {
            Sound_Manager.Instance.Play_Sound("Whack");

            hammer.SetActive(true);
            tween.Kill();
            hammer.transform.position = Input.mousePosition;

            Get_Obj().anchoredPosition = hammer_rect.anchoredPosition;
            hammer.transform.position = Input.mousePosition + new Vector3(Screen.width / 5, 0, 0);

            hammer.transform.rotation = Quaternion.Euler(0, 180, 50);
            tween = hammer.transform.DORotate(new Vector3(0, 180, 0), 0.5f)
                    .SetEase(Ease.OutElastic)
                    .OnComplete(() =>
                    {
                        hammer.SetActive(false);
                    });
        }
    }
    RectTransform Get_Obj()
    {
        i++;
        if (i == 10) i = 0;
        if (!HIT_pool[i].gameObject.activeSelf)
        {
            HIT_pool[i].gameObject.SetActive(true);
            return HIT_pool[i];
        }
        

        return null;
    }
    //void show_effect(RectTransform rect)
    //{
    //    GameObject obj = Get_Obj();
    //    obj.gameObject.SetActive(true);
    //    obj.transform.GetComponent<RectTransform>().anchoredPosition = rect.anchoredPosition;
    //}
}
