using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AI_BOT : MonoBehaviour
{
    internal enum drive_type
    {
        front_wheel_drive,
        rear_wheel_frive,
        all_wheel_drive
    }
    [SerializeField]
    private drive_type drive;
    [SerializeField]
    private float speed_value;

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
    public GameObject parent_target;
    public GameObject parent_target_2;
    public List<GameObject> target;
    public List<GameObject> target_2;
    private int target_value;
    private bool impact;
    private float impact_value;

    private bool before_player;
    public GameObject player;
    private int round_value;

    public GameObject bread_BOT;
    public static float bread_count = 1;
    private bool finish = false;

    public GameObject banana;
    public GameObject bread;

    public TextMeshProUGUI Notification;
    private float skill = 0;

    public List<ParticleSystem> vfx;
    private bool smoke = false;

    public GameObject game_over;

    private bool swap_target = false;

    public List<GameObject> win_effect;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ALL = transform.Find("ALL").gameObject;
        target_value = 0;
        impact = false;
        before_player = true;
        round_value = 1;
        banana.SetActive(false);
        bread.SetActive(false);
        update_speed_max();
    }
    void FixedUpdate()
    {
        if (game_over.activeSelf)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.drag = 0.001f + 0.001f * speed_value;
            add_force_down();
            //animate_wheels();

            Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 7 + new Vector3(0, 0.3f, 0), Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward + new Vector3(0, 0.0f, 0), out hit, 7f))
            {
                if (hit.collider.CompareTag("BOT") || hit.collider.CompareTag("player"))
                {
                    if (!swap_target)
                    {
                        swap_target = true;
                        if (Vector3.Distance(transform.position, target_2[target_value].transform.position) < 10 && target_value < 59) target_value++;
                        Invoke("swap_target_false", 0.5f);
                    }
                }
            }
            if (impact)
            {
                move_vehical(1);
                //steer_vehical(-Mathf.Clamp(angle_calculation() / 10 , -1, 1));

                steer_vehical(impact_value);
            }
            else
            {
                move_vehical(1);
                steer_vehical(Mathf.Clamp(angle_calculation() / 10, -1, 1));
            }
            update_rank_player();
            //if (!bread.activeSelf) bread.transform.position = transform.position + transform.forward * 5 + new Vector3(0, 1f, 0);

            if (!smoke && speed_value < 1)
            {
                smoke = true;
                vfx[4].Play();
                vfx[5].Play();
            }
            if (smoke && speed_value > 10)
            {
                smoke = false;
                vfx[4].Stop();
                vfx[5].Stop();
            }
            if (speed_max == 30 && vfx[2].isStopped)
            {
                vfx[2].Play();
                vfx[3].Play();
            }
            if (speed_max == 20 && vfx[2].isPlaying)
            {
                vfx[2].Stop();
                vfx[3].Stop();
            }
        }
    }
    float angle_calculation()
    {
        Vector3 forward = transform.forward;
        Vector3 toTarget;
        if (!swap_target)
        {
            toTarget = target[target_value].transform.position - transform.position;
        }
        else
        {
            toTarget = target_2[target_value].transform.position - transform.position;
        }
        float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);
        return angle;
    }
    void bread_plus()
    {
        bread_count = 1;
    }






    public void move_vehical(float vertical)
    {
        speed_value = rb.velocity.magnitude;
        if (speed_value < speed_max)
        {
            for (int i = 0; i < 4; i++)
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
                case drive_type.rear_wheel_frive:
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
                wheels[i].brakeTorque = speed_value * 100;
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

        for (int i = 0; i < 4; i++)
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
    void update_rank_player()
    {
        if (before_player && target_value < car_ctrl.rank_point && round_value == car_ctrl.round_point)
        {
            before_player = false;
            car_ctrl.rank--;
        }
        if (!before_player && target_value > car_ctrl.rank_point && round_value == car_ctrl.round_point)
        {
            before_player = true;
            car_ctrl.rank++;
        }

        if (target_value == car_ctrl.rank_point && round_value == car_ctrl.round_point)
        {
            Vector3 forward = transform.forward;
            Vector3 toTarget = player.transform.position - transform.position;

            float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);
            if (!before_player && (angle < -90 || angle > 90))
            {
                before_player = true;
                car_ctrl.rank++;
                update_speed_max();
            }
            if (before_player && angle > -90 && angle < 90)
            {
                before_player = false;
                car_ctrl.rank--;
            }
        }
    }

    void update_speed_max()
    {
        float temp = Vector3.Distance(transform.position, player.transform.position);
        if (temp > 50)
        {
            if (temp < 100) speed_max = 20 - (temp - 50) / 5;
            else speed_max = 10;
        }
        if (temp < 50 && speed_max < 20) speed_max = 20;
        if (before_player) Invoke("update_speed_max", 1f);
    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target_point"))
        {
            target_value = other.gameObject.name[7] - '0';
            if (other.gameObject.name[8] != ')')
            {
                target_value *= 10;
                target_value += other.gameObject.name[8] - '0';
            }

            switch (target_value) // boost nếu player đứng đầu
            {
                case 19:
                case 20:

                case 27:
                case 28:
                case 29:

                case 34:

                case 36:
                case 37:

                case 44:
                case 45:

                case 47:
                case 48:

                case 54:
                case 55:

                case 59:
                    if (car_ctrl.rank <= 2 && !before_player)
                    {
                        motor_torque = 4000;
                        speed_max = 30;
                    }
                    break;
                case 21:
                case 30:
                case 35:
                case 38:
                case 46:
                case 49:
                case 56:
                    //case 0:
                    motor_torque = 2500;
                    speed_max = 20;
                    break;
            }

            if (target_value + 1 > target.Count - 1)
            {
                finish = true;
                target_value = 0;
                round_value++;
            }
            else
            {
                target_value++;
            }
            Bread.target_value_obj[gameObject.name[5] - '0'] = target_value;
        }
        if (other.CompareTag("chest"))
        {
            other.gameObject.SetActive(false);
            if (skill == 0)
            {
                skill = Random.Range(1, 4);
                switch (skill)
                {
                    case 1:
                        Invoke("skill_1", 2);
                        break;
                    case 2:
                        Invoke("skill_2", Random.Range(2, 5));
                        break;
                    case 3:
                        Invoke("skill_3", Random.Range(2, 5));
                        break;
                }
            }
        }
        if (other.CompareTag("banana") && other.gameObject != bread)
        {
            other.gameObject.SetActive(false);
            Body.gameObject.layer = LayerMask.NameToLayer("car_hide");
            StartCoroutine(SpinCar());
            CancelInvoke("hide_notification");
            if (other.gameObject.name[other.gameObject.name.Length - 2] == '7')
            {
                Notification.text = "<color=green>Player </color>" + "use " + other.gameObject.name.Substring(0, other.gameObject.name.Length - 4) +
                                " " + "<color=red>" + gameObject.name + "</color>";
            }
            else
            {
                Notification.text = "<color=red>" + "BOT " + other.gameObject.name[other.gameObject.name.Length - 2] + "</color>" +
                                    " use " + other.gameObject.name.Substring(0, other.gameObject.name.Length - 4) +
                                    " " + "<color=red>" + gameObject.name + "</color>";

            }
            Invoke("hide_notification", 3);
        }
        if (other.CompareTag("Ramp"))
        {
            wheels[0].motorTorque = wheels[1].motorTorque = 2500;
        }
        if (other.CompareTag("Finish") && finish)
        {
            foreach (GameObject obj in win_effect) obj.SetActive(true);
            motor_torque = 0;
            car_ctrl.rank_finish++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ramp"))
        {
            wheels[0].motorTorque = wheels[1].motorTorque = 0;
        }
    }
    private IEnumerator SpinCar()
    {
        float duration = 1.0f; // Thời gian xoay là 1 giây
        float timeElapsed = 0; // Thời gian đã trôi qua
        float spinSpeed = 720f; // Tốc độ xoay (được đo bằng độ/giây)

        while (timeElapsed < duration)
        {
            rb.velocity = rb.transform.forward * speed_max * (duration - timeElapsed) / duration;
            timeElapsed += Time.deltaTime; // Cập nhật thời gian đã trôi qua
            ALL.transform.Rotate(0, spinSpeed * Time.deltaTime, 0); // Xoay xe
            yield return null; // Chờ đến frame tiếp theo
        }
        ALL.transform.rotation = transform.rotation;
        Body.gameObject.layer = LayerMask.NameToLayer("car");
    }

    private void OnCollisionEnter(Collision collision)
    {
        CancelInvoke("set_impact_false");
        //if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("player") || collision.gameObject.CompareTag("BOT"))
        //{
        //    Vector3 forward = transform.forward;
        //    Vector3 toTarget = collision.transform.position - transform.position;

        //    float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);

        //    if (angle > -45 && angle < 45) impact = true;

        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, transform.forward, out hit, 3.5f))
        //    {
        //        impact = true;
        //    }
        //}

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 direction = transform.position - collision.contacts[0].point;
            direction = direction.normalized;

            // Áp dụng lực đẩy ngược
            rb.AddForce(direction * 500 * speed_value, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("BOT") || collision.gameObject.CompareTag("player"))
        {
            Vector3 direction = transform.position - collision.contacts[0].point;
            if (Vector3.Angle(direction, transform.forward) > 115)
            {
                direction = direction.normalized;

                rb.velocity = Vector3.zero;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * -100 * speed_value, ForceMode.Impulse);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Invoke("set_impact_false", 0.5f);
    }
    void set_impact_false()
    {
        impact = false;
    }
    void speed_down()
    {
        speed_max = 20;
        motor_torque = 2500;
    }
    void skill_1()
    {
        speed_max = 30;
        motor_torque = 4000;
        Invoke("speed_down", 3);
        skill = 0;
        //if (gameObject.name == "BOT (0)") Debug.Log("1");
    }
    void skill_2()
    {
        banana.SetActive(true);
        banana.transform.position = transform.position - transform.forward * 3 + new Vector3(0, 0.5f, 0);
        skill = 0;
        //if (gameObject.name == "BOT (0)") Debug.Log("2");
    }
    void skill_3()
    {
        bread.SetActive(true);
        bread.transform.position = transform.position + transform.forward * 3 + new Vector3(0, 1f, 0);
        skill = 0;
        //if (gameObject.name == "BOT (0)") Debug.Log("3");
    }
    void hide_notification()
    {
        Notification.text = "";
    }

    public void AssignChildrenToTarget()
    {
        target.Clear();
        foreach (Transform child in parent_target.transform)
        {
            target.Add(child.gameObject);
        }
        target_2.Clear();
        foreach (Transform child in parent_target_2.transform)
        {
            target_2.Add(child.gameObject);
        }
    }
    void swap_target_false()
    {
        swap_target = false;
        if (Vector3.Distance(transform.position, target[target_value].transform.position) < 10) target_value++;
        if (target_value == target.Count) target_value = 0;
    }

    //void OnDrawGizmos()
    //{
    //    if (target == null || target.Count < 2)
    //        return;

    //    Gizmos.color = Color.green; // Đặt màu cho Gizmos. Bạn có thể thay đổi màu này theo ý muốn.

    //    for (int i = 0; i < target.Count - 1; i++)
    //    {
    //        if (target[i] == null || target[i + 1] == null)
    //            continue;

    //        Gizmos.DrawLine(target[i].transform.position, target[i + 1].transform.position);
    //    }
    //}

}
