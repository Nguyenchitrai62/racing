using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI three_second_text;
    public TextMeshProUGUI money_value;
    public TextMeshProUGUI score_value_text;
    public TextMeshProUGUI money_claim_value;

    public GameObject UI_game_over;
    public GameObject UI_three_second;

    public Button myButton;

    private Animator anim;
    private Rigidbody rb;
    private float start_time;
    private float start_game_over;
    private float speed;
    private float score = 0;
    private float old_score;
    private float three_second;

    public GameObject pause_bt;

    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;

    private void Awake()
    {
        Sound_Manager.Instance.Play_Music("BGM-Casual", 0);
        Sound_Manager.Instance.Play_Music("BGM-Casual", 1);
        canvas_controller.active_all_minigame = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //anim.SetInteger("state", 5);

        start_time = -3;
        speed = 4;
        UI_game_over.SetActive(false);
        //UI_three_second.SetActive(true);
        three_second = 3;

        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
    }
    private void Start()
    {
        rb.velocity = new Vector3(0, 0, speed);
        dem_nguoc();
        //anim.SetInteger("state", 5);
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
    void FixedUpdate()
    {
        if (rb.velocity.y != 10) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 0.5f, rb.velocity.z);
        if (transform.position.y < 1) transform.GetComponent<BoxCollider>().isTrigger = false;
        if (three_second < 0)
        {
            if (rb.isKinematic && !UI_game_over.activeSelf)
            {
                rb.isKinematic = false;
                rb.velocity = new Vector3(0, 0, speed);
                UI_three_second.SetActive(false);
            }


            if (UI_game_over.activeSelf)
            {
                claim_x1_text.text = (score * 1).ToString();
                claim_x2_text.text = (score * 2).ToString();

                pause_bt.SetActive(false);
                score_value_text.text = score.ToString();

                if (score == 0 && myButton.gameObject.activeSelf)
                {
                    myButton.onClick.Invoke();
                    myButton.gameObject.SetActive(false);
                }
                rb.isKinematic = true;
            }
            speed_up();
            control();
        }
        else
        {
            rb.isKinematic = true;
        }
        canvas_controller.money = Mathf.Round(canvas_controller.money);
        PlayerPrefs.SetFloat("money", Mathf.Round(canvas_controller.money));
        money_value.text = canvas_controller.money.ToString();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" && anim.GetInteger("state") != 7)
        {
            Effect_Manager.Instance.Start_white_flash();
            Sound_Manager.Instance.Play_Music("BGM-Casual", 0);
            Sound_Manager.Instance.Play_Sound("Oof");
            Invoke("play_game_lost", 1);

            anim.SetInteger("state", 7);
            transform.GetComponent<BoxCollider>().isTrigger = true;
            start_game_over = Time.time;
            //score_value_text.text = "SCORE : " + score.ToString();
            money_claim_value.text = score.ToString();
        }
    }
    public void speed_up()
    {
        old_score = score;
        score = Mathf.Floor((transform.position.z - 3) / 7);
        if (score < 0) score = 0;
        else score_text.text = score.ToString();
        if (score % 2 == 0 && old_score != score)
        {
            speed *= 1.05f;
            rb.velocity = new Vector3(0, 0, speed);
        }
        if (old_score != score) Sound_Manager.Instance.Play_Sound("PointUp");

    }
    public void control()
    {
        if (anim.GetInteger("state") != 7)
        {
            if (Time.time - start_time > 0 && Time.time - start_time < 0.5f)
            {
                //anim.SetInteger("state", 0);
            }
            if (Time.time - start_time > 0.5f)
            {
                anim.SetInteger("state", 5);
            }
        }
        else
        {
            if (Time.time - start_game_over > 2 && !UI_game_over.activeSelf)
            {
                UI_game_over.SetActive(true);
                PlayerPrefs.SetFloat("score_plappy_bird", score);
                if (score == 0)
                {
                    myButton.onClick.Invoke();
                }
            }
        }
    }
    public void junp()
    {
        if (anim.GetInteger("state") != 7 && Input.GetMouseButtonDown(0))
        {
            Sound_Manager.Instance.Play_Sound("Booing");
            Vector3 temp = rb.velocity;
            temp.y = 10;
            rb.velocity = temp;
            start_time = Time.time;
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
        DOVirtual.DelayedCall(1.2f, plus_10, true);
        Invoke("load_scene", 1.5f);
    }
    public void claimx2()
    {
        money_claim_value.text = (score * 2).ToString();
        DOVirtual.DelayedCall(1.2f, plus_10x2, true);
        Invoke("load_scene", 1.5f);
    }
    public void load_scene()
    {
        SceneManager.LoadScene("plappy_bird");
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
