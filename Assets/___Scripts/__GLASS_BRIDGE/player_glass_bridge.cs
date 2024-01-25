using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_glass_bridge : MonoBehaviour
{
    public List<GameObject> map;
    public List<GameObject> glass_left;
    public List<GameObject> glass_right;
    public GameObject finish;
    private int cur_row_glass = 0;
    private bool on_ground = true;

    public GameObject left_bt;
    public GameObject right_bt;
    public GameObject game_over;
    public GameObject start;

    public TextMeshProUGUI money_claim_text;
    public TextMeshProUGUI monney_value_text;
    public Button myButton;

    public static int map_value = 0;
    public static float highlight_duration = 1;
    public static bool next = false;

    public static float score = 0;

    public GameObject Show_safe_glass_bt;

    public static float count_down_time_value = 60;
    public TextMeshProUGUI count_down_time_text;
    private float level_value = 1;
    public TextMeshProUGUI level_text;
    public TextMeshProUGUI level_value_text;

    public GameObject GO;

    public GameObject fade_out;
    public GameObject back_bt;
    public GameObject pause_bt;

    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;

    public List<GameObject> win_effect;

    float time_onground_false;

    private Animator anim;
    private void Reset()
    {
#if UNITY_EDITOR
        Load_Component();
        Load_Map();
#endif
    }
    void Load_Component()
    {
        back_bt = GameObject.Find("Canvas").transform.Find("back").gameObject;
        pause_bt = GameObject.Find("Canvas").transform.Find("pause").gameObject;
        GO = GameObject.Find("Canvas").transform.Find("CENTER/GO").gameObject;
        level_value_text = GameObject.Find("Canvas").transform.Find("game_over/Image/Cup/level_value_text").GetComponent<TextMeshProUGUI>();
        level_text = GameObject.Find("Canvas").transform.Find("TOP/level_text").gameObject.GetComponent<TextMeshProUGUI>();
        count_down_time_text = GameObject.Find("Canvas").transform.Find("TOP/count_down_time_text").GetComponent<TextMeshProUGUI>();
        Show_safe_glass_bt = GameObject.Find("Canvas").transform.Find("CENTER/Help_bt").gameObject;
        fade_out = GameObject.Find("Fade_out");
        money_claim_text = GameObject.Find("Canvas").transform.Find("game_over/Image/money_claim/money_claim_text").GetComponent<TextMeshProUGUI>();
        myButton = GameObject.Find("Canvas").transform.Find("game_over/Image/Mybutton").GetComponent<Button>();
        monney_value_text = GameObject.Find("Canvas").transform.Find("money/TOP/money/money_value").GetComponent<TextMeshProUGUI>();
        money_claim_text = GameObject.Find("Canvas").transform.Find("game_over/Image/money_claim/money_claim_text").GetComponent<TextMeshProUGUI>();
        left_bt = GameObject.Find("Canvas").transform.Find("LEFT_bt").gameObject;
        right_bt = GameObject.Find("Canvas").transform.Find("RIGHT_bt").gameObject;
        game_over = GameObject.Find("Canvas").transform.Find("game_over").gameObject;
        start = GameObject.Find("Canvas").transform.Find("start").gameObject;
        GameObject temp = GameObject.Find("MAP").gameObject;
        foreach (Transform child in temp.transform)
        {
            map.Add(child.gameObject);
        }
    }
    void Load_Map()
    {
        glass_left.Clear();
        glass_right.Clear();
        if (map_value >= 3 && map_value < 6)
        {
            map[0].SetActive(false);
            map[1].SetActive(true);
        }
        if (map_value >= 6)
        {
            map[0].SetActive(false);
            map[2].SetActive(true);
        }
        for (int j = 0; j < 3; j++)
        {
            if (map[j].activeSelf)
            {
                finish = GameObject.Find("MAP").transform.Find(map[j].name.ToString() + "/Finish").gameObject;
                GameObject parent_glass = GameObject.Find("MAP").transform.Find(map[j].name.ToString() + "/GLASS").gameObject;
                int i = 0;
                foreach (Transform child in parent_glass.transform)
                {
                    if (i == 0)
                    {
                        glass_right.Add(child.gameObject);
                        i = 1;
                    }
                    else
                    {
                        glass_left.Add(child.gameObject);
                        i = 0;
                    }
                }
            }
        }
    }
    private void Awake()
    {
        Sound_Manager.Instance.stop_all_sound_effect();
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);

        anim = GetComponent<Animator>();
        canvas_controller.active_all_minigame = true;

        count_down_time_value = 60;
        score = 0;
        level_value = 1;
        if (next)
        {
            next = false;
            Time.timeScale = 1;
            start.SetActive(false);
            back_bt.SetActive(false);
            pause_bt.SetActive(true);
            score = PlayerPrefs.GetFloat("score", 0);
            level_value = PlayerPrefs.GetFloat("level_value", 1);
            level_text.text = "Level " + level_value.ToString();
        }
        Load_Map();
        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
        monney_value_text.text = canvas_controller.money.ToString();
        StartCoroutine(ProcessHighlighting());
    }
    IEnumerator ProcessHighlighting()
    {
        for (int i = 0; i < glass_left.Count; i++)
        {
            if (i == 0) yield return new WaitForSeconds(1);
            else yield return new WaitForSeconds(highlight_duration);
            if (Random.Range(0, 2) == 0)
            {
                glass_right[i].GetComponent<BoxCollider>().isTrigger = true;
                Highlight(glass_left[i]);
            }
            else
            {
                glass_left[i].GetComponent<BoxCollider>().isTrigger = true;
                Highlight(glass_right[i]);
            }
        }
        Invoke("isplaying", highlight_duration + 0.2f);
    }
    void isplaying()
    {
        left_bt.SetActive(true);
        right_bt.SetActive(true);
        GO.SetActive(true);
        Show_safe_glass_bt.SetActive(true);
        time_game_over();
    }









    private void FixedUpdate()
    {
        if (game_over.activeSelf)
        {
            claim_x1_text.text = (score * 1).ToString();
            claim_x2_text.text = (score * 2).ToString();

            pause_bt.SetActive(false);
            highlight_duration = 1;
            level_value_text.text = (level_value - 1).ToString();
            if (score == 0 && myButton.gameObject.activeSelf)
            {
                myButton.onClick.Invoke();
                myButton.gameObject.SetActive(false);
            }
        }
        else
        {
            money_claim_text.text = score.ToString();
        }
        monney_value_text.text = canvas_controller.money.ToString();
    }







    public void jump_left()
    {
        if (on_ground && cur_row_glass < glass_left.Count)
        {
            on_ground = false;
            time_onground_false = Time.time;
            transform.DOJump(glass_left[cur_row_glass].transform.position, 2, 1, 0.75f);
            anim.SetInteger("state", 9);
            cur_row_glass++;
        }
        if (on_ground && cur_row_glass == glass_left.Count)
        {
            on_ground = false;
            time_onground_false = Time.time;
            transform.DOJump(finish.transform.position, 2, 1, 1);
            anim.SetInteger("state", 9);
        }
    }
    public void jump_right()
    {
        if (on_ground && cur_row_glass < glass_right.Count)
        {
            on_ground = false;
            time_onground_false = Time.time;
            transform.DOJump(glass_right[cur_row_glass].transform.position, 2, 1, 0.75f);
            anim.SetInteger("state", 9);
            cur_row_glass++;
        }
        if (on_ground && cur_row_glass == glass_left.Count)
        {
            on_ground = false;
            time_onground_false = Time.time;
            transform.DOJump(finish.transform.position, 2, 1, 0.75f);
            anim.SetInteger("state", 9);
        }
    }
    public void Highlight(GameObject obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material objMaterial = obj.GetComponent<Renderer>().material;
            Color originalColor = objMaterial.color;

            objMaterial.DOColor(Color.green, highlight_duration / 2f)
                .OnComplete(() =>
                {
                    objMaterial.DOColor(originalColor, highlight_duration / 2f);
                });
        }
    }

    void plus_10()
    {
        canvas_controller.money += score;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }

    void plus_10x2()
    {
        canvas_controller.money += score * 2;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }

    public void claim()
    {
        Invoke("plus_10", 1.2f);
        Invoke("replay", 1.5f);
    }

    public void claimx2()
    {
        money_claim_text.text = (score * 2).ToString();
        Invoke("plus_10x2", 1.2f);
        Invoke("replay", 1.5f);
    }
    public void back()
    {
        Time.timeScale = 1;
        map_value = 0;
        highlight_duration = 1;
        SceneManager.LoadScene("game_1");
    }
    public void replay()
    {
        SceneManager.LoadScene("glass_bridge");
    }
    public void show_safe_glass()
    {
        Sound_Manager.Instance.Play_Sound("Scanner");
        foreach (GameObject obj in glass_left)
        {
            if (!obj.GetComponent<BoxCollider>().isTrigger)
            {
                Material temp = obj.GetComponent<Renderer>().material;
                temp.color = Color.green;
            }
        }
        foreach (GameObject obj in glass_right)
        {
            if (!obj.GetComponent<BoxCollider>().isTrigger)
            {
                Material temp = obj.GetComponent<Renderer>().material;
                temp.color = Color.green;
            }
        }
    }
    void time_game_over()
    {
        count_down_time_value -= 0.05f;
        if (count_down_time_value >= 10)
        {
            count_down_time_text.text = "0 : " + count_down_time_value.ToString("F2");
        }
        else
        {
            if (count_down_time_value <= 5) Sound_Manager.Instance.Play_sound_effect(0);
            count_down_time_text.text = "0 : 0" + count_down_time_value.ToString("F2");
        }

        if (count_down_time_value == 0)
        {
            Sound_Manager.Instance.Stop_sound_effect(0);
            game_over.SetActive(true);
        }
        else
        {
            if (!game_over.activeSelf) Invoke("time_game_over", 0.05f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !game_over.activeSelf)
        {
            Sound_Manager.Instance.Play_sound_effect(12);
            Sound_Manager.Instance.Play_sound_effect(17);
            anim.SetInteger("state", 0);

            foreach (GameObject obj in win_effect) obj.SetActive(true);
            Invoke("win_state", 1f);
        }
        if (collision.gameObject.CompareTag("destroy"))
        {
            map_value = 0;
            highlight_duration = 1;
            game_over.SetActive(true);
        }
        if (collision.gameObject.CompareTag("safe"))
        {
            if (Time.time - time_onground_false > 0.5f)
            {
                on_ground = true;
                anim.SetInteger("state", 0);
            }
            Sound_Manager.Instance.Play_Sound("PointUp");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("safe"))
        {
            other.gameObject.SetActive(false);
            Effect_Manager.Instance.Start_white_flash();
            Sound_Manager.Instance.Play_Music("BGM-Action", 0);
            Sound_Manager.Instance.Play_Sound("GlassBreak");
            Invoke("play_game_lost", 1);
        }
    }
    void win_state()
    {
        highlight_duration *= 0.8f;
        if (highlight_duration < 0.4f) highlight_duration = 0.4f;
        map_value++;

        next = true;
        fade_out.SetActive(true);
        Invoke("replay", 0.8f);
        score += 10;
        PlayerPrefs.SetFloat("score", score);
        level_value++;
        PlayerPrefs.SetFloat("level_value", level_value);
    }
    void play_game_lost()
    {
        Sound_Manager.Instance.Play_Sound("GameLost");
    }
}
