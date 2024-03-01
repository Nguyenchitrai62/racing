using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class canvas_controller : MonoBehaviour
{
    public static canvas_controller Instance;
    public bool main_menu;
    public bool play;
    public bool game_over;
    public bool win;
    public bool mini_game;
    public bool change_skin;

    public static float cur_score;
    public static float max_score;

    public static float speed = 5f;
    public static float range = 1;
    public static float money = 0;
    public static float level = 1;
    public static float skin_free = 1;
    public static int[] remove_obj = new int[100];

    public TextMeshProUGUI speed_level;
    public TextMeshProUGUI speed_value;
    public TextMeshProUGUI range_level;
    public TextMeshProUGUI range_value;
    public TextMeshProUGUI money_value;
    public TextMeshProUGUI level_value;
    public TextMeshProUGUI money_claim_value;
    public TextMeshProUGUI level_in_play;

    public TextMeshProUGUI XX_percen;
    public TextMeshProUGUI score_value_text;
    public TextMeshProUGUI time_minigame;

    public float _time_minigame;

    public GameObject canvas_skin;
    public GameObject get_skin_free;
    public GameObject full_100_percen_button;
    public GameObject image_skin;
    public GameObject new_baby;
    public GameObject unlock_random;
    public GameObject plus_500;
    public GameObject cur_mission;
    public GameObject home_map;
    public GameObject UI_start;
    public GameObject canvas;
    public GameObject lost_reward_bt;
    public RectTransform process_get_new_baby;

    public GameObject all_minigame;
    public GameObject unlock_skin;

    public List<GameObject> map;
    public List<GameObject> lock_button;


    private GameObject end_minigame;
    private GameObject start_minigame;
    Transform popup_settings;
    Transform _main_menu;
    Transform _play;
    Transform _game_over;
    Transform _win;
    Transform _mini_game;

    int free_skin_value;
    bool check_skin_free;

    public RectTransform objectToPool;
    private List<RectTransform> objectPool;
    public RectTransform recttranform;

    public Button win_claim_x1;
    public Button win_claim_x2;
    public Button win_100_percen;

    public List<TextMeshProUGUI> text_cup;
    public GameObject pause_minigame;
    public static bool active_all_minigame = false;
    private void Awake()
    {
        //Debug.Log(Screen.currentResolution.refreshRate);

        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        Instance = this;
        //load
        money = PlayerPrefs.GetFloat("money", 0);
        range = PlayerPrefs.GetFloat("range", 1);
        speed = PlayerPrefs.GetFloat("speed", 5);
        level = PlayerPrefs.GetFloat("level", 1);
        skin_free = PlayerPrefs.GetFloat("skin_free", 1);
        Load_Array();

        //khởi tạo trạng thái ban đầu
        canvas_skin.SetActive(false);
        cur_score = 0;
        main_menu = true;
        play = false;
        game_over = false;
        win = false;
        mini_game = false;
        change_skin = false;
        check_skin_free = true;

        _main_menu = transform.Find("main_menu");
        _play = transform.Find("play");
        _game_over = transform.Find("game_over");
        _win = transform.Find("win");
        _mini_game = transform.Find("whack_mouse");

        popup_settings = transform.Find("popup_setting");
        popup_settings.gameObject.SetActive(false);

        get_skin_free.SetActive(false);
        if (active_all_minigame)
        {
            all_minigame.SetActive(true);
            map[0].transform.parent.gameObject.SetActive(false);
            active_all_minigame = false;
        }
        else all_minigame.SetActive(false);

        end_minigame = transform.Find("whack_mouse").Find("end_minigame").gameObject;
        start_minigame = GameObject.Find("Canvas").transform.Find("whack_mouse/start").gameObject;
        end_minigame.SetActive(false);

        int i = 0;
        while (remove_obj[i] > 0)
        {
            lock_button[remove_obj[i] - 1].SetActive(false);
            lock_button.RemoveAt(remove_obj[i] - 1);
            i++;
        }
        if (lock_button.Count > 0)
        {
            free_skin_value = Random.Range(0, lock_button.Count);
        }
        else
        {
            skin_free = 0;
        }
        home_map.SetActive(false);
        process_get_new_baby.sizeDelta = new Vector2(672 * 0.25f * (skin_free - 1), process_get_new_baby.sizeDelta.y);
    }
    private void Start()
    {
        active_map();


        //mouse_minigame.score_mini_game = PlayerPrefs.GetFloat("score_mini_game",0f);
        //if (mouse_minigame.score_mini_game > 0)
        //{
        //    Invoke("get_money", 0.5f);
        //}

        objectPool = new List<RectTransform>();
        for (int i = 0; i < 8; i++)
        {
            RectTransform obj = Instantiate(objectToPool);
            obj.transform.SetParent(canvas.transform, false);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }
    }
    void FixedUpdate()
    {
        money_value.text = money.ToString();
        if (main_menu)
        {
            _main_menu.gameObject.SetActive(true);
            _play.gameObject.SetActive(false);
            _game_over.gameObject.SetActive(false);
            _win.gameObject.SetActive(false);
            _mini_game.gameObject.SetActive(false);

            float speed_temp = Mathf.RoundToInt((speed - 4.7f) / 0.3f);

            speed_level.text = "Level " + speed_temp.ToString();
            speed_value.text = speed_temp.ToString() + "00";

            range_level.text = "Level " + range.ToString();
            range_value.text = range.ToString() + "00";

            level_value.text = "Level " + level.ToString();
        }
        if (play)
        {
            main_menu = false;
            _main_menu.gameObject.SetActive(false);
            _play.gameObject.SetActive(true);
            level_in_play.text = "Level " + level.ToString();
        }
        if (game_over)
        {
            text_cup[1].text = cur_score.ToString() + "/" + max_score.ToString();
            _play.gameObject.SetActive(false);
            play = false;
            _game_over.gameObject.SetActive(true);
        }
        if (win)
        {
            text_cup[0].text = max_score.ToString() + "/" + max_score.ToString();
            if (check_skin_free)
            {
                Vector2 temp = process_get_new_baby.sizeDelta;
                switch (skin_free)
                {
                    case 0:
                        check_skin_free = false;
                        new_baby.SetActive(false);
                        full_100_percen_button.SetActive(false);
                        break;
                    case 1:
                        if (temp.x < 672 * 0.25f)
                        {
                            new_baby.SetActive(true);
                            temp.x += 3;
                            process_get_new_baby.sizeDelta = temp;
                        }
                        else
                        {
                            process_get_new_baby.sizeDelta = new Vector2(672 * 0.25f, process_get_new_baby.sizeDelta.y);
                            check_skin_free = false;
                        }
                        XX_percen.text = "25%";
                        break;
                    case 2:
                        if (temp.x < 672 * 0.5f)
                        {
                            new_baby.SetActive(true);
                            temp.x += 6;
                            process_get_new_baby.sizeDelta = temp;
                        }
                        else
                        {
                            process_get_new_baby.sizeDelta = new Vector2(672 * 0.5f, process_get_new_baby.sizeDelta.y);
                            check_skin_free = false;
                        }
                        XX_percen.text = "50%";
                        break;
                    case 3:
                        if (temp.x < 672 * 0.75f)
                        {
                            new_baby.SetActive(true);
                            temp.x += 9;
                            process_get_new_baby.sizeDelta = temp;
                        }
                        else
                        {
                            process_get_new_baby.sizeDelta = new Vector2(672 * 0.75f, process_get_new_baby.sizeDelta.y);
                            check_skin_free = false;
                        }
                        XX_percen.text = "75%";
                        break;
                    case 4:
                        if (temp.x < 672)
                        {
                            new_baby.SetActive(true);
                            temp.x += 12;
                            process_get_new_baby.sizeDelta = temp;
                            win_claim_x1.enabled = false;
                            win_claim_x2.enabled = false;
                            win_100_percen.enabled = false;
                        }
                        else
                        {
                            process_get_new_baby.sizeDelta = new Vector2(672, process_get_new_baby.sizeDelta.y);
                            check_skin_free = false;
                            Invoke("show_get_skin_free", 0.5f);
                        }
                        XX_percen.text = "100%";
                        break;
                }
            }
            if (get_skin_free.activeSelf && !lost_reward_bt.activeSelf)
            {
                Invoke("active_lost_reward", 2);
            }
            _play.gameObject.SetActive(false);
            play = false;
            _win.gameObject.SetActive(true);
        }
        if (mini_game)
        {
            _mini_game.gameObject.SetActive(true);
            _main_menu.gameObject.SetActive(false);
            float temp = 15 - (Time.time - _time_minigame);
            if (end_minigame.activeSelf && temp > 0) _time_minigame = Time.time - 15;
            if (start_minigame.activeSelf) _time_minigame = Time.time;
            if (temp > 0 && temp < 14.9f)
            {
                if (transform.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceCamera)
                transform.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;

                Sound_Manager.Instance.Play_Music("BGM-Casual", 1);
                if (Mathf.Round(temp) < 10)
                {
                    time_minigame.text = (Mathf.Round(temp * 100) / 100).ToString("F0");
                    if (Mathf.Round(temp) <= 5) Sound_Manager.Instance.Play_sound_effect(0);
                }
                else
                {
                    time_minigame.text = (Mathf.Round(temp * 100) / 100).ToString("F0");
                }
            }
            if (temp <= 0)
            {
                //transform.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                time_minigame.text = "0 : 0.00";
                Sound_Manager.Instance.Play_sound_effect(18);
                Sound_Manager.Instance.Play_Music("BGM-Casual", 0);
                Sound_Manager.Instance.Stop_sound_effect(0);
                PlayerPrefs.SetFloat("score" + gameObject.scene.name, mouse_minigame.score_mini_game);

                end_minigame.SetActive(true);
                pause_minigame.SetActive(false);
                //score_value_text.text = mouse_minigame.score_mini_game.ToString();
                if (temp > -0.1)
                {
                    money_claim_value.text = mouse_minigame.score_mini_game.ToString();
                    PlayerPrefs.SetFloat("high_score_whack_mouse", mouse_minigame.score_mini_game);
                }
            }
        }
        if (change_skin)
        {
            _main_menu.gameObject.SetActive(false);
            _play.gameObject.SetActive(false);
            _game_over.gameObject.SetActive(false);
            _win.gameObject.SetActive(false);
        }
        //if (lock_button.Count == 0 && unlock_random.activeSelf)
        //{
        //    Vector3 temp = plus_500.GetComponent<RectTransform>().anchoredPosition;
        //    temp.x = -540;
        //    plus_500.GetComponent<RectTransform>().anchoredPosition = temp;
        //    unlock_random.SetActive(false);
        //}
    }






























    void Save_Array()
    {
        for (int i = 0; i < remove_obj.Length; i++)
        {
            PlayerPrefs.SetInt("remote_obj_" + i, remove_obj[i]);
        }
    }
    void Load_Array()
    {
        for (int i = 0; i < remove_obj.Length; i++)
        {
            remove_obj[i] = PlayerPrefs.GetInt("remote_obj_" + i);
        }
    }

    public void restart()
    {
        SceneManager.LoadSceneAsync("game_1");
        Time.timeScale = 1;
    }
    public void get_money()
    {
        money += mouse_minigame.score_mini_game;
        PlayerPrefs.SetFloat("money", money);
        mouse_minigame.score_mini_game = 0;
        PlayerPrefs.SetFloat("score_mini_game", mouse_minigame.score_mini_game);
        money_value.gameObject.SetActive(true);
    }
    public void claim()
    {
        for (int i = 0; i < 8; i++)
        {
            objectPool[i].gameObject.SetActive(true);
            objectPool[i].anchoredPosition = objectPool[i].transform.InverseTransformPoint(Input.mousePosition) + new Vector3(0, 300, 0);
            Vector3 temp = objectPool[i].anchoredPosition + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));

            objectPool[i].DOAnchorPos(temp, 0.5f);
            objectPool[i].DOAnchorPos(recttranform.anchoredPosition, 0.7f).SetDelay(0.5f);
        }
        Invoke("claim_100", 1.2f);
        Invoke("restart", 1.5f);
    }
    public void claim_100()
    {
        money += 100;
        PlayerPrefs.SetFloat("money", money);
        level++;
        PlayerPrefs.SetFloat("level", level);
        skin_free++;
        PlayerPrefs.SetFloat("skin_free", skin_free);
    }
    public void claim_ad()
    {
        for (int i = 0; i < 8; i++)
        {
            objectPool[i].gameObject.SetActive(true);
            objectPool[i].anchoredPosition = objectPool[i].transform.InverseTransformPoint(Input.mousePosition) + new Vector3(0, 300, 0);
            Vector3 temp = objectPool[i].anchoredPosition + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));

            objectPool[i].DOAnchorPos(temp, 0.5f);
            objectPool[i].DOAnchorPos(recttranform.anchoredPosition, 0.7f).SetDelay(0.5f);
        }
        Invoke("claim_200", 1.2f);
        Invoke("restart", 1.5f);
    }
    public void claim_200()
    {
        money += 200;
        PlayerPrefs.SetFloat("money", money);
        level++;
        PlayerPrefs.SetFloat("level", level);
        skin_free++;
        PlayerPrefs.SetFloat("skin_free", skin_free);
    }
    public void speed_up()
    {
        Sound_Manager.Instance.Play_Sound("Upgrade");
        float speed_temp = Mathf.RoundToInt((speed - 4.7f) / 0.3f);
        if (money >= speed_temp * 100)
        {
            money -= speed_temp * 100;
            PlayerPrefs.SetFloat("money", money);
            speed += 0.3f;
            PlayerPrefs.SetFloat("speed", speed);
        }
        else
        {
            speed += 0.3f;
            PlayerPrefs.SetFloat("speed", speed);
        }
    }
    public void range_up()
    {
        Sound_Manager.Instance.Play_Sound("Upgrade");
        if (money >= range * 100)
        {
            money -= range * 100;
            PlayerPrefs.SetFloat("money", money);
            range += 1;
            PlayerPrefs.SetFloat("range", range);
        }
        else
        {
            range += 1;
            PlayerPrefs.SetFloat("range", range);
        }
    }
    public void tap_to_start()
    {
        play = true;
        player_controler.cout_down = true;
    }
    public void open_popup_setting()
    {
        popup_settings.gameObject.SetActive(true);
    }
    public void open_popup_shop()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
        {
            canvas_skin.SetActive(true);
            change_skin = true;
            main_menu = false;  
        });
    }
    public void skip()
    {
        restart();
        level++;
        PlayerPrefs.SetFloat("level", level);
    }
    public void cong_500()
    {
        money += 500;
        PlayerPrefs.SetFloat("money", money);
    }
    public void random_skin()
    {
        if (lock_button.Count > 0 && money >= 500)
        {
            int random_value = Random.Range(0, lock_button.Count);
            lock_button[random_value].SetActive(false);
            lock_button.RemoveAt(random_value);
            int i = 0;
            while (remove_obj[i] > 0)
            {
                i++;
            }
            remove_obj[i] = random_value + 1;
            Save_Array();
            money -= 500;
            PlayerPrefs.SetFloat("money", money);
        }
        else
        {
            Money.cur_value = 501;
        }
    }
    public void unlock_skin_bt()
    {
        int j = 0;
        int temp = player_controler.unlock_skin;
        while (remove_obj[j] > 0)
        {
            if (remove_obj[j] < temp)
            {
                temp--;
            }
            j++;
        }
        if (money >= 500)
        {
            int random_value = temp - 1;
            lock_button[random_value].SetActive(false);
            lock_button.RemoveAt(random_value);
            int i = 0;
            while (remove_obj[i] > 0)
            {
                i++;
            }
            remove_obj[i] = random_value + 1;
            Save_Array();
            money -= 500;
            PlayerPrefs.SetFloat("money", money);
            unlock_skin.SetActive(false);
            PlayerPrefs.SetInt("skin_value", player_controler.skin_value);
        }
        else
        {
            Money.cur_value = 501;
        }
    }
    public void claim_new_skin()
    {
        lock_button[free_skin_value].SetActive(false);
        lock_button.RemoveAt(free_skin_value);
        int i = 0;
        while (remove_obj[i] > 0)
        {
            i++;
        }
        remove_obj[i] = free_skin_value + 1;
        Save_Array();

        get_skin_free.SetActive(false);
        skin_free = 0;
    }
    public void lose_reward()
    {
        get_skin_free.SetActive(false);
        skin_free = 0;
    }
    public void home()
    {
        foreach (GameObject i in map)
        {
            i.SetActive(false);
        }
        home_map.SetActive(true);
        play = true;
        cur_mission.SetActive(false);
    }
    //public void play_mini_game()
    //{
    //    main_menu = false;
    //    all_minigame.SetActive(false);
    //    mini_game = true;
    //    _time_minigame = Time.time;
    //    Time.timeScale = 0;
    //    UI_start.SetActive(true);
    //}
    public void plappy_bird()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("plappy_bird"));
    }
    public void racing()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("racing"));
    }
    public void shotter()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("shotter"));
    }
    public void squid_game()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("squid_game"));
    }
    public void glass_bridge()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("glass_bridge"));
    }
    public void kart()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("kart"));
    }
    public void tug()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("tug"));
    }
    public void show_all_minigame()
    {
        all_minigame.SetActive(true);
    }
    public void start_game()
    {
        Time.timeScale = 1;
        UI_start.SetActive(false);
    }
    public void pause()
    {
        Time.timeScale = 0;
    }
    public void resume()
    {
        Time.timeScale = 1;
    }
    public void full_100_percen()
    {
        check_skin_free = true;
        skin_free += 2;
        if (skin_free > 4) skin_free = 4;

        //new_baby.SetActive(false);
        //get_skin_free.SetActive(true);
        //image_skin.GetComponent<Image>().sprite = lock_button[free_skin_value].transform.parent.GetComponent<Image>().sprite;
    }
    public void active_lost_reward()
    {
        lost_reward_bt.SetActive(true);
    }
    void show_get_skin_free()
    {
        new_baby.SetActive(false);
        get_skin_free.SetActive(true);
        image_skin.GetComponent<Image>().sprite = lock_button[free_skin_value].transform.parent.Find("Image").GetComponent<Image>().sprite;
        win_claim_x1.enabled = true;
        win_claim_x2.enabled = true;
        win_100_percen.enabled = true;
    }
    void active_map()
    {
        int temp = (int)(level % map.Count);

        max_score = map[temp].gameObject.name[8] - '0';
        map[temp].SetActive(true);
    }
































    //whack a mouse
    public void plus_10_whack_mouse()
    {
        money += mouse_minigame.score_mini_game;
        PlayerPrefs.SetFloat("money", money);
    }
    public void plus_10x2_whack_mouse()
    {
        money += mouse_minigame.score_mini_game * 2;
        PlayerPrefs.SetFloat("money", money);
    }
    public void claim_whack_mouse()
    {
        Invoke("plus_10_whack_mouse", 1.2f);
        //Invoke("restart", 1.5f);
    }
    public void claimx2_whack_mouse()
    {
        money_claim_value.text = (2 * mouse_minigame.score_mini_game).ToString();
        Invoke("plus_10x2_whack_mouse", 1.2f);
        //Invoke("load_scene", 1.5f);
    }
}
