using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class car_ctrl : MonoBehaviour
{
    internal enum drive_type
    {
        front_wheel_drive,
        rear_wheel_drive,
        all_wheel_drive
    }
    [SerializeField]
    private drive_type drive;
    [SerializeField]
    public static float speed_value;
    private Input_manager IM;

    public List<WheelCollider> wheels;
    public List<GameObject> wheel_mesh;

    //public float radius = 6;
    public float down_force_value = 100;
    private Rigidbody rb;
    private GameObject ALL;
    public float motor_torque = 2500;
    public float steering_max = 40;
    public float speed_max = 20;
    public GameObject Body;

    public float[] slip = new float[4];

    public static int rank_point;
    public static float round_point;
    public static int rank;
    public TextMeshProUGUI rank_text;

    public GameObject banana;
    public GameObject bread;

    private int skill = 0;
    public TextMeshProUGUI skill_text;
    public TextMeshProUGUI LAP_text;
    public TextMeshProUGUI SPEED_text;

    public List<GameObject> ALL_skill;
    private bool boost = false;

    private int[] check_point = new int[100];
    public TextMeshProUGUI place_text;
    public TextMeshProUGUI place_value_text;
    public TextMeshProUGUI money_claim_text;
    public TextMeshProUGUI money_value_text;

    public GameObject game_over;

    public static float nearest;
    public List<GameObject> BOT;

    private bool finish;
    public static int rank_finish;
    public TextMeshProUGUI Notification;

    public List<ParticleSystem> vfx;

    public GameObject Anime_Speedlines;

    public Button myButton;
    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;

    public List<GameObject> win_effect;

    private string rank_string;

    private void Awake()
    {
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);

        canvas_controller.active_all_minigame = true;

        rb = GetComponent<Rigidbody>();
        IM = GetComponent<Input_manager>();
        ALL = transform.Find("ALL").gameObject;
        rank_point = 0;
        round_point = 1;
        rank = 8;
        banana.SetActive(false);
        for (int i = 0; i < 100; i++) check_point[i] = 0;
        game_over.SetActive(false);
        finish = false;

        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
        money_value_text.text = canvas_controller.money.ToString();
        rank_finish = 1;
    }

    void FixedUpdate()
    {
        rb.drag = 0.001f + 0.001f * speed_value;
        add_force_down();
        //animate_wheels();

        if (finish)
        {
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = speed_value * 80;
            }
        }
        else
        {
            steer_vehical(IM.horizontal);
            move_vehical(IM.vertical);
        }

        //get_friction();

        //if (!bread.activeSelf) bread.transform.position = transform.position + transform.forward * 5 + new Vector3(0, 1f, 0);
        //if (rank == 1) nearest = BOT_nearest();
        if (!game_over.activeSelf)
        {
            switch(rank)
            { 
                case 1:
                    rank_text.text = "<color=#F0C322>" + rank.ToString() + "</color>" + " / 8";
                    break;
                case 2:
                    rank_text.text = "<color=#738995>" + rank.ToString() + "</color>" + " / 8";
                    break;
                case 3:
                    rank_text.text = "<color=#E8620F>" + rank.ToString() + "</color>" + " / 8";
                    break;
                default:
                    rank_text.text = "<color=#FFFFFF>" + rank.ToString() + "</color>" + " / 8";
                    break;
            }
            if (rank_string != rank_text.text)
            {
                rank_text.transform.DOScale(Vector3.one * 1.5f, 0.25f) // Zoom in
               .OnComplete(() => // Sau khi zoom in hoàn thành
               {
                   rank_text.transform.DOScale(Vector3.one, 0.25f); // Zoom out
               });
            }
            rank_string = rank_text.text;

            SPEED_text.text = "Speed " + Mathf.Round(speed_value).ToString();
            LAP_text.text = round_point.ToString();
        }
        else
        {
            money_value_text.text = canvas_controller.money.ToString();
            if (place_value_text.text == "8th place") myButton.onClick.Invoke();
        }

        if (IM.vertical > 0)
        {
            if (!vfx[4].isPlaying && speed_value > 1 && speed_value < 10)
            {
                vfx[4].Play();
                vfx[5].Play();
            }
        }
        if (vfx[4].isPlaying && speed_value > 10 || speed_value < 1)
        {
            vfx[4].Stop();
            vfx[5].Stop();
        }
        if (speed_value > 1 && !game_over.activeSelf)
        {
            if (speed_max == 20)
            {
                Sound_Manager.Instance.Play_sound_effect(26);
                Sound_Manager.Instance.Stop_sound_effect(27);
            }
            if (speed_max == 12)
            {
                Sound_Manager.Instance.Play_sound_effect(27);
            }
        }
        else
        {
            Sound_Manager.Instance.Stop_sound_effect(26);
            Sound_Manager.Instance.Stop_sound_effect(27);
        }

    }

    public void move_vehical(float vertical)
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            skill = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            skill = 2;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            skill = 3;
        }
        if (Input.GetKey(KeyCode.E))
        {
            //Sound_Manager.Instance.sound_effect[3].GetComponent<AudioSource>().enabled = true;
            if (skill == 1)
            {
                drive = drive_type.all_wheel_drive;
                Sound_Manager.Instance.sound_effect[26].GetComponent<AudioSource>().volume = 1f;
                boost = true;
                vfx[2].Play();
                vfx[3].Play();
                motor_torque = 4000;
                Invoke("speed_down", 3);
            }
            if (skill == 2)
            {
                Sound_Manager.Instance.Play_Sound("Banana");
                banana.SetActive(true);
                banana.transform.position = transform.position - transform.forward * 3 + new Vector3(0, 0.5f, 0);
            }
            if (skill == 3)
            {
                bread.SetActive(true);
                bread.transform.position = transform.position + transform.forward * 5 + new Vector3(0, 1f, 0);
            }
            skill = 0;
            skill_text.text = "Skill : ";
            foreach (GameObject obj in ALL_skill) obj.SetActive(false);
        }

        speed_value = rb.velocity.magnitude;
        if (speed_value < speed_max || (boost && speed_value < 30))
        {
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].brakeTorque = 0;
            }
            switch (drive)
            {
                case drive_type.front_wheel_drive:
                    for (int i = 0; i < 2; i++)
                    {
                        wheels[i].motorTorque = vertical * (motor_torque / 2);
                    }
                    break;
                case drive_type.rear_wheel_drive:
                    for (int i = 2; i < 4; i++)
                    {
                        wheels[i].motorTorque = vertical * (motor_torque / 2);
                    }
                    break;
                case drive_type.all_wheel_drive:
                    for (int i = 0; i < 4; i++)
                    {
                        wheels[i].motorTorque = vertical * (motor_torque / 4);
                    }
                    break;
            }
        }
        else
        {
            for (int i = 2; i < 4; i++)
            {
                wheels[i].brakeTorque = speed_value * 100;
            }
        }

        if (vertical == 0)
        {
            for (int i = 2; i < 4; i++)
            {
                wheels[i].brakeTorque = speed_value * 50;
            }
        }

    }
    public void steer_vehical(float horizontal)
    {
        wheels[0].steerAngle = wheels[1].steerAngle = steering_max * horizontal;
    }
    public void animate_wheels()
    {
        Vector3 wheel_position = Vector3.zero;
        Quaternion wheel_rotation = Quaternion.identity;

        for (int i = 0; i < wheel_mesh.Count - 1; i++)
        {
            wheels[i].GetWorldPose(out wheel_position, out wheel_rotation);
            wheel_mesh[i].transform.position = wheel_position;
            wheel_mesh[i].transform.rotation = wheel_rotation;
        }
    }
    public void add_force_down()
    {
        rb.AddForce(-transform.up * down_force_value * rb.velocity.magnitude);
    }
    public void get_friction()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            WheelHit wheel_hit;
            wheels[i].GetGroundHit(out wheel_hit);

            slip[i] = wheel_hit.sidewaysSlip;
        }
    }
    public void speed_down()
    {
        drive = drive_type.rear_wheel_drive;
        wheels[0].motorTorque = wheels[1].motorTorque = 0;
        Sound_Manager.Instance.sound_effect[26].GetComponent<AudioSource>().volume = 0.5f;
        motor_torque = 2500;
        boost = false;
        vfx[2].Stop();
        vfx[3].Stop();
        Anime_Speedlines.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target_point"))
        {
            rank_point = other.gameObject.name[7] - '0';
            if (other.gameObject.name[8] != ')')
            {
                rank_point *= 10;
                rank_point += other.gameObject.name[8] - '0';
            }
            check_point[rank_point]++;
            if (rank_point == 59) // = target point (max)
            {
                rank_point = 0;
                bool check = true;
                for (int i = 0; i < 60; i++)
                {
                    if (check_point[i] == 0)
                    {
                        check = false;
                    }
                }
                if (check) round_point += 0.5f;
            }
            else
            {
                rank_point++;
            }
            Bread.target_value_obj[7] = rank_point;
        }
        if (other.CompareTag("banana") && other.gameObject != bread)
        {
            Effect_Manager.Instance.Start_white_flash();

            if (other.name.StartsWith("bread")) Sound_Manager.Instance.Play_Sound("BreadHit");
            else Sound_Manager.Instance.Play_Sound("Banana");
            other.gameObject.SetActive(false);
            Body.gameObject.layer = LayerMask.NameToLayer("car_hide");
            StartCoroutine(SpinCar());
            CancelInvoke("hide_notification");
            Notification.text = "<color=red>" + "BOT " + other.gameObject.name[other.gameObject.name.Length - 2] + "</color>" +
                                " use " + other.gameObject.name.Substring(0, other.gameObject.name.Length - 4) +
                                " " + "<color=green>" + gameObject.name + "</color>";
            Invoke("hide_notification", 3);
        }
        if (other.CompareTag("chest"))
        {
            Sound_Manager.Instance.Play_Sound("BuffBox");
            other.gameObject.SetActive(false);
            if (skill == 0)
            {
                if (rank == 1)
                {
                    skill = Random.Range(1, 3);
                }
                else
                {
                    skill = Random.Range(1, 4);
                }
                Invoke("random_skill", 2);
            }
        }
        if (other.CompareTag("Ramp"))
        {
            wheels[0].motorTorque = wheels[1].motorTorque = 2500;
        }
        if (other.CompareTag("Finish"))
        {
            finish = true;
            for (int i = 0; i < 60; i++)
            {
                if (check_point[i] == 0)
                {
                    finish = false;
                }
            }
            if (finish)
            {
                Sound_Manager.Instance.Play_Music("BGM-Action", 0);
                Sound_Manager.Instance.Play_Sound("Confetti");
                Invoke("play_Confetti", 0.5f);
                Invoke("play_Confetti", 1f);
                foreach (GameObject obj in win_effect) obj.SetActive(true);
                rank = rank_finish;
                if (rank <= 3) Invoke("play_game_win", 1);
                else Invoke("play_game_lost", 1);
                place_text.text = rank_finish.ToString() + " place";
                place_value_text.text = rank_finish.ToString() + "th place";
                switch (rank_finish)
                {
                    case 1:
                        money_claim_text.text = "50";
                        claim_x1_text.text = "50";
                        claim_x2_text.text = "100";
                        place_value_text.text = "<color=#F0C322>" + rank_finish.ToString() + "st place" + "</color>";
                        place_text.text = "<color=#F0C322>" + rank_finish.ToString() + "st place" + "</color>";
                        break;
                    case 2:
                        money_claim_text.text = "35";
                        claim_x1_text.text = "35";
                        claim_x2_text.text = "70";
                        place_value_text.text = "<color=#738995>" + rank_finish.ToString() + "nd place" + "</color>";
                        place_text.text = "<color=#738995>" + rank_finish.ToString() + "nd place" + "</color>";
                        break;
                    case 3:
                        money_claim_text.text = "25";
                        claim_x1_text.text = "25";
                        claim_x2_text.text = "50";
                        place_value_text.text = "<color=#E8620F>" + rank_finish.ToString() + "rd place" + "</color>";
                        place_text.text = "<color=#E8620F>" + rank_finish.ToString() + "rd place" + "</color>";
                        break;
                    case 4:
                    case 5:
                        money_claim_text.text = "10";
                        claim_x1_text.text = "10";
                        claim_x2_text.text = "20";
                        break;
                    case 6:
                    case 7:
                    case 8:
                        money_claim_text.text = "5";
                        claim_x1_text.text = "5";
                        claim_x2_text.text = "10";
                        break;
                }
                Invoke("show_game_over", 3);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ramp"))
        {
            wheels[0].motorTorque = wheels[1].motorTorque = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (speed_value > 1 && (IM.horizontal == -1 || IM.horizontal == 1))
        {
            Sound_Manager.Instance.Play_sound_effect(15);
            if (!vfx[6].isPlaying)
            {
                vfx[6].Play();
                vfx[7].Play();
            }
        }
        else
        {
            Sound_Manager.Instance.Stop_sound_effect(15);
            if (vfx[6].isPlaying)
            {
                vfx[6].Stop();
                vfx[7].Stop();
            }
        }
        if (other.CompareTag("racetrack") || other.CompareTag("target_point") || other.CompareTag("Ramp") || other.CompareTag("Finish"))
        {
            speed_max = 20;
            if (vfx[0].isPlaying)
            {
                vfx[0].Stop();
                vfx[1].Stop();
            }
        }
        else
        {
            speed_max = 12;
            if (vfx[0].isStopped && speed_value > 0.1f)
            {
                vfx[0].Play();
                vfx[1].Play();
            }
            if (vfx[0].isPlaying && speed_value < 0.1f)
            {
                vfx[0].Stop();
                vfx[1].Stop();
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 direction = transform.position - collision.contacts[0].point;
            direction = direction.normalized;

            // Áp dụng lực đẩy ngược
            rb.AddForce(direction * 500 * speed_value, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("BOT"))
        {
            Effect_Manager.Instance.Start_white_flash();

            Vector3 direction = transform.position - collision.contacts[0].point;
            if (Vector3.Angle(direction, transform.forward) > 115)
            {
                direction = direction.normalized;

                rb.velocity = Vector3.zero;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * -100 * speed_value, ForceMode.Impulse);
            }
        }
    }
    private IEnumerator SpinCar()
    {
        float duration = 1.0f;
        float timeElapsed = 0;
        float spinSpeed = 720f;

        while (timeElapsed < duration)
        {
            rb.velocity = rb.transform.forward * speed_max * (duration - timeElapsed) / duration;
            timeElapsed += Time.deltaTime;
            ALL.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
            yield return null;
        }
        ALL.transform.rotation = transform.rotation;
        Body.gameObject.layer = LayerMask.NameToLayer("car");
    }

    void random_skill()
    {
        if (skill == 1)
        {
            skill_text.text = "Skill : boost";
            ALL_skill[0].SetActive(true);
        }
        if (skill == 2)
        {
            skill_text.text = "Skill : banana";
            ALL_skill[1].SetActive(true);
        }
        if (skill == 3)
        {
            skill_text.text = "Skill : bread";
            ALL_skill[2].SetActive(true);
        }
    }
    public void USE_skill()
    {
        if (skill == 1)
        {
            drive = drive_type.all_wheel_drive;
            Sound_Manager.Instance.sound_effect[26].GetComponent<AudioSource>().volume = 1f;
            boost = true;
            vfx[2].Play();
            vfx[3].Play();
            Anime_Speedlines.SetActive(true);
            motor_torque = 4000;
            Invoke("speed_down", 3);
        }
        if (skill == 2)
        {
            banana.SetActive(true);
            banana.transform.position = transform.position - transform.forward * 3 + new Vector3(0, 0.5f, 0);
        }
        if (skill == 3)
        {
            bread.SetActive(true);
            bread.transform.position = transform.position + transform.forward * 3 + new Vector3(0, 1f, 0);
        }
        skill = 0;
        skill_text.text = "Skill : ";
        foreach (GameObject obj in ALL_skill) obj.SetActive(false);
    }
    public void show_game_over()
    {
        game_over.SetActive(true);
    }
    public void claim()
    {
        Invoke("claim_1", 1.2f);
        Invoke("replay", 1.5f);
    }
    public void claimx2()
    {
        int number;
        if (int.TryParse(money_claim_text.text, out number))
        {
            money_claim_text.text = (number * 2).ToString();
        }
        Invoke("claim_1", 1.2f);
        Invoke("replay", 1.5f);
    }
    void claim_1()
    {
        int number;
        if (int.TryParse(money_claim_text.text, out number))
        {
            canvas_controller.money += number;
            PlayerPrefs.SetFloat("money", canvas_controller.money);
        }
    }
    public void replay()
    {
        SceneManager.LoadScene("kart");
    }
    public float BOT_nearest()
    {
        float shortestDistance = Mathf.Infinity;
        int i_min = -1;
        for (int i = 0; i < BOT.Count; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, BOT[i].transform.position);
            if (Vector3.Distance(transform.position, BOT[i].transform.position) < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                i_min = i;
            }
        }
        return i_min;
    }
    void hide_notification()
    {
        Notification.text = "";
    }
    void play_game_lost()
    {
        Sound_Manager.Instance.Play_Sound("GameLost");
    }
    void play_game_win()
    {
        Sound_Manager.Instance.Play_Sound("GameWin");
    }
    void play_Confetti()
    {
        Sound_Manager.Instance.Play_Sound("Confetti");
    }
}
