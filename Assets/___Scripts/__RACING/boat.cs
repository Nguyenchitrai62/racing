using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class boat : MonoBehaviour
{
    //public Animator anim;
    public TextMeshProUGUI three_second_text;
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI money_value;
    public TextMeshProUGUI score_value_text;
    public TextMeshProUGUI money_claim_value;

    public GameObject UI_game_over;
    public GameObject UI_three_second;

    public Button myButton;

    private Rigidbody rb;
    //private float start_time = -3;
    private float speed;
    private float start_position;
    private float end_position;
    private bool is_click = false;
    private float target = 6;
    private float start_time = -3;
    private float three_second;
    private float score;
    private float start_game_over;

    private bool game_over = false;
    private int move = 1;

    public GameObject pause_bar;

    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;

    public Animator player_anim;
    private void Awake()
    {
        Sound_Manager.Instance.Play_Music("BGM-Casual", 0);
        Sound_Manager.Instance.Play_Music("BGM-Casual", 1);
        canvas_controller.active_all_minigame = true;

        three_second = 3;
        speed = 10;
        UI_game_over.SetActive(false);
        score = 0;

        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, speed);
        rb.isKinematic = true;

        dem_nguoc();
    }

    public void dem_nguoc()
    {
        if (three_second >= 0)
        {
            if (three_second > 0)
            {
                three_second_text.text = Mathf.Round(three_second).ToString();
            }

            three_second -= 1;

            Invoke("dem_nguoc", 1f);
        }
    }

    void Update()
    {
        player_anim.SetInteger("state", 6);
        if (three_second < 0)
        {
            if (!game_over)
            {
                if (rb.isKinematic)
                {
                    Sound_Manager.Instance.Play_sound_effect(5);
                    rb.isKinematic = false;
                    rb.velocity = new Vector3(0, 0, speed);
                    UI_three_second.SetActive(false);
                }
                rb.velocity = new Vector3(0, 0, speed);
                controler();
            }
            else
            {
                claim_x1_text.text = (score * 1).ToString();
                claim_x2_text.text = (score * 2).ToString();

                rb.velocity = Vector3.zero;
                if (Time.time - start_game_over > 2 && !UI_game_over.activeSelf)
                {
                    if (score == 0)
                    {
                        myButton.onClick.Invoke();
                    }
                    PlayerPrefs.SetFloat("score" + gameObject.scene.name, score);
                    UI_game_over.SetActive(true);
                    pause_bar.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        if (UI_game_over.activeSelf)
        {
            if (score == 0)
            {
                myButton.onClick.Invoke();
            }
            game_over = true;
        }
    }
    private void FixedUpdate()
    {
        if (!game_over)
        {
            if (Time.time - start_time > 0 && Time.time - start_time < 0.5f)
            {
                float temp = Mathf.Lerp(transform.position.x, target, 10 * Time.deltaTime);
                transform.position = new Vector3(temp, transform.position.y, transform.position.z);
            }
            if (Time.time - start_time > 0.5f)
            {
                transform.position = new Vector3(target, transform.position.y, transform.position.z);
            }
        }

        canvas_controller.money = Mathf.Round(canvas_controller.money);
        PlayerPrefs.SetFloat("money", Mathf.Round(canvas_controller.money));
        money_value.text = canvas_controller.money.ToString();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            Effect_Manager.Instance.Start_white_flash();
            Sound_Manager.Instance.Play_Music("BGM-Casual", 0);
            Sound_Manager.Instance.Stop_sound_effect(5);
            Sound_Manager.Instance.Play_Sound("Crash");
            Invoke("play_game_lost", 1);

            start_game_over = Time.time;
            game_over = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Sound_Manager.Instance.Play_Sound("PointUp");

        score += 1;
        score_text.text = score.ToString();
        score_value_text.text = score.ToString();
        money_claim_value.text = score.ToString();
        if (score % 2 == 0)
        {
            speed *= 1.1f;
        }
        //rb.velocity = new Vector3(0, 0, speed);
    }

    public void controler()
    {
        if (Input.GetMouseButtonDown(0) && is_click == false && !pause_bar.activeSelf)
        {
            start_position = Input.mousePosition.x;
            is_click = true;
        }
        if (is_click && move == 1 && Input.mousePosition.x - start_position > 5 && transform.position.x < 8)
        {
            Sound_Manager.Instance.Play_Sound("BoatTurn");

            target = Mathf.Round((transform.position.x + 3) / 3) * 3;
            start_time = Time.time;
            move = 0;
        }
        if (is_click && move == 1 && Input.mousePosition.x - start_position < -5 && transform.position.x > 4)
        {
            Sound_Manager.Instance.Play_Sound("BoatTurn");

            target = Mathf.Round((transform.position.x - 3) / 3) * 3;
            start_time = Time.time;
            move = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            end_position = Input.mousePosition.x;
            is_click = false;
            move = 1;
        }
    }
    public void plus_10()
    {
        canvas_controller.money += score;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    public void plus_10x2()
    {
        canvas_controller.money += score * 2;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    public void claim()
    {
        Invoke("plus_10", 1.2f);
        Invoke("load_scene", 1.5f);
    }
    public void claimx2()
    {
        money_claim_value.text = (score * 2).ToString();
        Invoke("plus_10x2", 1.2f);
        Invoke("load_scene", 1.5f);
    }
    public void load_scene()
    {
        SceneManager.LoadScene("racing");
    }
    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("game_1");
    }
    void play_game_lost()
    {
        Sound_Manager.Instance.Play_Sound("GameLost");
    }
}
