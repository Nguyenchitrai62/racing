using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_ctrl : MonoBehaviour
{
    public FloatingJoystick joystick;
    private float speed = 3;

    private Rigidbody rb;
    private Animator anim;

    public GameObject game_over;
    public static float time_value;
    public TextMeshProUGUI green_red_light_text;
    public Image red;

    public GameObject enemy;
    public static bool is_alive;
    private bool safe = false;

    public GameObject bullet;

    public TextMeshProUGUI monney_claim_text;
    public TextMeshProUGUI monney_value_text;
    //public GameObject replay_bt;
    //public GameObject home_bt;

    public TextMeshProUGUI count_down_time_text;
    private float count_down_time_value;
    public GameObject start;
    private bool start_prev = true;

    public Button myButton;
    public LineRenderer laze;

    public TextMeshProUGUI place_value_text;
    public static float place_value;

    public GameObject projector;

    public GameObject alert;

    public float update_time_down;

    private bool shield = false;
    public GameObject shield_effect;

    public List<GameObject> win_effect;
    private void Awake()
    {
        Sound_Manager.Instance.stop_all_sound_effect();
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);

        canvas_controller.active_all_minigame = true;

        place_value = 1;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        time_value = 0;
        is_alive = true;

        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
        monney_value_text.text = canvas_controller.money.ToString();

        projector.SetActive(false);
        update_time_down = 1;
    }

    private void Update()
    {
        if (shield) shield_effect.SetActive(true);
        else shield_effect.SetActive(false);
        player_controller();
        Destroy_Player();

        if (start_prev && !start.activeSelf)
        {
            start_prev = false;
            count_down_time_value = 60;
            time_game_over();
            Invoke("time_countdown", 0.1f);
        }
        monney_value_text.text = canvas_controller.money.ToString();
        if (game_over.activeSelf) time_value = 50;
    }

    public void time_countdown()
    {
        if (time_value == 1)
        {
            Sound_Manager.Instance.sound_effect[35].GetComponent<AudioSource>().pitch = 1.05f / update_time_down;
            Sound_Manager.Instance.Play_sound_effect(35);
        }

        time_value += 1f;
        red.fillAmount = time_value / 100f;
        if (time_value == 96) enemy_rotate(-180);
        if (time_value < 100)
        {
            Invoke("time_countdown", 0.05f * update_time_down);
        }
        else
        {
            Sound_Manager.Instance.Stop_sound_effect(35);
            green_red_light_text.text = "Red Light";
            Sound_Manager.Instance.Play_Sound("redlight");
            projector.SetActive(true);
            Invoke("death_time", 2f);
            Invoke("greenlight_sound", 1.5f);
        }
    }
    void greenlight_sound()
    {
        Sound_Manager.Instance.Play_Sound("greenlight");
    }
    public void death_time() // red light
    {
        if (update_time_down > 0.6f) update_time_down -= 0.1f;
        time_value = 0;
        enemy_rotate(0);
        green_red_light_text.text = "Green Light";
        projector.SetActive(false);
        time_countdown();
    }

    void time_game_over() //60s count down
    {
        count_down_time_value-= 0.05f;
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
            Time.timeScale = 0;
            game_over.SetActive(true);
            myButton.onClick.Invoke();
        }
        else
        {
            Invoke("time_game_over", 0.05f);
        }
    }

    void player_controller()
    {
        if (!game_over.activeSelf && is_alive && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            anim.SetInteger("state", 1);
            rb.velocity = new Vector3(joystick.Horizontal * speed, rb.velocity.y, joystick.Vertical * speed);
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        if (Input.GetMouseButtonUp(0) && is_alive)
        {
            anim.SetInteger("state", 0);
            rb.velocity = Vector3.zero;
        }
    }
    void Destroy_Player()
    {
        if (time_value == 100)
        {
            Vector3 direction = enemy.transform.position - transform.position;
            Ray ray = new Ray(transform.position, direction.normalized);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, direction.magnitude))
            {
                if (hit.collider.CompareTag("safe"))
                {
                    safe = true;
                }
                else
                {
                    safe = false;
                }
            }
        }
        if (is_alive && time_value == 100 && rb.velocity.magnitude > 0.001f && !safe && transform.position.z > -12)
        {
            if (!Flicker_Effect.is_flicker) show_laze();

            if (!shield && !Flicker_Effect.is_flicker)
            {
                Effect_Manager.Instance.Start_white_flash();
                Sound_Manager.Instance.Play_Music("BGM-Action", 0);
                Sound_Manager.Instance.Play_Sound("Oof");
                Sound_Manager.Instance.Stop_sound_effect(35);
                Invoke("play_game_lost", 1);

                alert.SetActive(true);
                rb.velocity = Vector3.zero;
                Invoke("alert_active_false", 0.5f);

                is_alive = false;

                anim.SetInteger("state", 16);
                //Ragdoll ragdollScript = gameObject.GetComponent<Ragdoll>();
                //if (ragdollScript != null)
                //{
                //    ragdollScript.SetRagdollState(true);
                //}

                Invoke("show_game_over", 2f);
            }

            if (shield)
            {
                Sound_Manager.Instance.Play_Sound("ShieldBreak");
                shield = false;
                Flicker_Effect.is_flicker = true;
                Invoke("is_flicker_equal_false", 1.5f);
            }
        }
    }
    void alert_active_false()
    {
        alert.SetActive(false);
    }
    void show_laze()
    {
        Sound_Manager.Instance.Play_Sound("Lasershot");
        laze.gameObject.SetActive(true);
        laze.transform.position = transform.position + new Vector3(0, 1, 0);
        laze.SetPosition(0, transform.position + new Vector3(0, 1, 0));
        laze.SetPosition(1, new Vector3(0, 5f, 55));
        Invoke("hide_laze", 0.25f);
    }
    void hide_laze()
    {
        laze.gameObject.SetActive(false);
    }
    public void show_game_over()
    {
        game_over.SetActive(true);
        myButton.onClick.Invoke();
        monney_claim_text.text = "0";
        place_value_text.text = "0";
        Time.timeScale = 0;
    }

    public void back()
    {
        Sound_Manager.Instance.Stop_sound_effect(35);
        Time.timeScale = 1;
        SceneManager.LoadScene("game_1");
    }

    public void replay()
    {
        SceneManager.LoadScene("squid_game");
    }

    void plus_10()
    {
        canvas_controller.money += 100;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }

    void plus_10x2()
    {
        canvas_controller.money += 200;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }

    public void claim()
    {
        Invoke("plus_10", 1.2f);
        Invoke("replay", 1.5f);
    }

    public void claimx2()
    {
        monney_claim_text.text = "200";
        Invoke("plus_10x2", 1.2f);
        Invoke("replay", 1.5f);
    }

    public void enemy_rotate(float angle)
    {
        enemy.transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), 0.2f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish") && transform.position.z > 53)
        {
            Sound_Manager.Instance.Play_Sound("Confetti");
            Sound_Manager.Instance.Stop_sound_effect(35);
            Invoke("play_game_win", 1);

            foreach (GameObject obj in win_effect) obj.SetActive(true);
            Invoke("win_state", 1f);
            rb.isKinematic = true;
        }
        if (other.CompareTag("safe"))
        {
            safe = false;
        }
    }
    public void speed_up()
    {
        Sound_Manager.Instance.Play_Sound("SpeedUp");
        speed = 4f;
    }
    public void shield_up()
    {
        Sound_Manager.Instance.Play_Sound("ShieldUp");
        shield = true;
    }
    void is_flicker_equal_false()
    {
        Flicker_Effect.is_flicker = false;
    }
    void win_state()
    {
        CancelInvoke();
        monney_claim_text.text = "100";
        place_value_text.text = place_value.ToString();
        game_over.SetActive(true);
    }
    void play_game_lost()
    {
        Sound_Manager.Instance.Play_Sound("GameLost");
    }
    void play_game_win()
    {
        Sound_Manager.Instance.Play_Sound("GameWin");
    }
}
