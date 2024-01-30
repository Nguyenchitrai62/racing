using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mouse_minigame : MonoBehaviour
{
    public RectTransform objectToPool;
    public GameObject canvas;
    public RectTransform recttranform;
    public static float score_mini_game;
    public TextMeshProUGUI score_mini_game_text;
    public money_anim money_anim;   

    private List<RectTransform> objectPool;

    private Vector2 start_mouse;
    private Vector2 end_mouse;

    private float time_random_start = -10;
    private float time_start = -10;
    private int random_value;
    private int random_object;
    private float cur_money;
    private float time_show = 2; // thời gian chờ để thụt xuống
    private float time_show_clock = 4;// bộ đếm thời gian time_show


    private GameObject[] game_obj = new GameObject[2];

    public GameObject start;
    public GameObject end_minigame;

    public Button myButton;

    private GameObject funny_rat;
    private GameObject sad_rat;

    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;
    private void OnEnable()
    {
        canvas_controller.active_all_minigame = true;
        score_mini_game = 0;
    }

    private void Awake()
    {
        //objectPool = new List<RectTransform>();
        //for (int i = 0; i < 8; i++)
        //{
        //    RectTransform obj = Instantiate(objectToPool);
        //    obj.transform.SetParent(canvas.transform, false);
        //    obj.gameObject.SetActive(false);
        //    objectPool.Add(obj);
        //}

        game_obj[0] = transform.Find("mouse_1").gameObject;
        game_obj[1] = transform.Find("mouse_2").gameObject;
        funny_rat = transform.Find("mouse_1/funny_rat").gameObject;
        sad_rat = transform.Find("mouse_1/sad_rat").gameObject;

        start_mouse = game_obj[0].GetComponent<RectTransform>().anchoredPosition;
        end_mouse = game_obj[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 200);
        score_mini_game = 0;
    }

    void FixedUpdate()
    {
        if (!end_minigame.activeSelf && !start.activeSelf)
        {
            if (random_value < 70 && Time.time - time_random_start > 1)
            {
                random_value = Random.Range(0, 100);
                time_random_start = Time.time;
                random_object = Random.Range(0, 100);
                if (random_value >= 70)
                {
                    time_start = Time.time;
                }
            }
            // thời gian trồi lên và chui xuống
            if (random_object < 80)
            {
                up_down(game_obj[0]);
            }
            else
            {
                up_down(game_obj[1]);
            }

            if (cur_money != score_mini_game)
            {
                if (cur_money > score_mini_game)
                {
                    transform.parent.transform.DOShakePosition(1, new Vector3(10f, 0f, 0f))
                        .OnComplete(() => transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector3.zero);
                }
                cur_money = score_mini_game;
            }

            if (time_show_clock >= 0)
            {
                time_show_clock -= Time.deltaTime;
            }
            else
            {
                time_show *= 0.8f;
                time_show_clock = 4;
            }
        }
        else
        {
            claim_x1_text.text = (score_mini_game * 1).ToString();
            claim_x2_text.text = (score_mini_game * 2).ToString();
        }
        if (game_obj[0].transform.localPosition.y < 10 && sad_rat.activeSelf)
        {
            sad_rat.SetActive(false);
            funny_rat.SetActive(true);
        }
        if (game_obj[1].transform.localPosition.y < 55)
        {
            game_obj[1].SetActive(false);
        }
        else game_obj[1].SetActive(true);
        score_mini_game_text.text = score_mini_game.ToString();
        if (score_mini_game == 0 && end_minigame.activeSelf && myButton.gameObject.activeSelf) myButton.onClick.Invoke();
    }
    public void hide_mouse()
    {
        if (funny_rat.activeSelf)
        {
            money_anim.AnimateObjects();
            time_start = Time.time - time_show + 0.5f;
            plus_10();
        }
    }
    public void mouse_o()
    {
        Sound_Manager.Instance.Play_Sound("Invalid");
        Sound_Manager.Instance.Play_Sound("Mole_Laugh");
        if (score_mini_game >= 1)
        {
            score_mini_game -= 2;
            if (score_mini_game < 0) score_mini_game = 0;
            score_mini_game_text.text = score_mini_game.ToString();
        }

    }

    public void mouse_up(GameObject obj)
    {
        Vector2 temp = Vector2.Lerp(obj.GetComponent<RectTransform>().anchoredPosition, end_mouse, 5 * Time.deltaTime);
        obj.GetComponent<RectTransform>().anchoredPosition = temp;
    }
    public void mouse_down(GameObject obj)
    {
        Vector2 temp = Vector2.Lerp(obj.GetComponent<RectTransform>().anchoredPosition, start_mouse, 10 * Time.deltaTime);
        obj.GetComponent<RectTransform>().anchoredPosition = temp;
    }

    public void up_down(GameObject obj)
    {
        if (Time.time - time_start > 0 && Time.time - time_start < time_show)
        {
            mouse_up(obj);
        }
        if (Time.time - time_start > time_show && Time.time - time_start < 2 * time_show)
        {
            mouse_down(obj);
            time_random_start = Time.time;
        }
        if (Time.time - time_start > 4)
        {
            random_value = 0;
        }
    }
    public void plus_10()
    {
        //foreach (RectTransform obj in objectPool)
        //{
        //    obj.gameObject.SetActive(false);
        //}
        score_mini_game += 10f;
        PlayerPrefs.SetFloat("score_mini_game", score_mini_game);
    }
}
