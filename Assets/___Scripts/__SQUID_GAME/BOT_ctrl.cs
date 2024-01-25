using DG.Tweening;
using UnityEngine;

public class BOT_ctrl : MonoBehaviour
{
    private Rigidbody rb;

    private Animator anim;

    public GameObject start;
    private float random_X = 0;

    bool isHitRight;
    bool isHitLeft;
    bool end_game = false;
    bool safe = false;
    private Tweener myTweener;

    public float time_to_die;
    public LineRenderer laze;
    public GameObject enemy;

    public GameObject game_over;

    public GameObject alert;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
        Invoke("random_randomX", 1);
        time_to_die = Time.time + Random.Range(0, 40);
    }
    private void Update()
    {
        BOT_controler();
        Destroy_BOT();
    }

    void BOT_controler()
    {
        RaycastHit hitRight;
        if (Physics.Raycast(transform.position + new Vector3(0.3f, 0, 0), Quaternion.Euler(0f, 30, 0f) * transform.forward, out hitRight, 1.7f)
            && !hitRight.collider.isTrigger && !hitRight.collider.CompareTag("BOT")) isHitRight = true;
        else isHitRight = false;

        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position + new Vector3(-0.3f, 0, 0), Quaternion.Euler(0f, -30, 0f) * transform.forward, out hitLeft, 1.7f)
            && !hitLeft.collider.isTrigger && !hitLeft.collider.CompareTag("BOT")) isHitLeft = true;
        else isHitLeft = false;

        Debug.DrawRay(transform.position + new Vector3(0.3f, 0, 0), (Quaternion.Euler(0f, 30, 0f) * transform.forward) * 1.7f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(-0.3f, 0, 0), (Quaternion.Euler(0f, -30, 0f) * transform.forward) * 1.7f, Color.blue);

        if ((player_ctrl.time_value < 95f || Time.time > time_to_die) && !start.activeSelf && !end_game && !game_over.activeSelf)
        {
            if (isHitLeft || isHitRight)
            {
                myTweener.Kill();
                CancelInvoke("random_randomX");
            }
            if (!(isHitLeft && isHitRight))
            {
                if (isHitRight && random_X - 0.05f >= -2) random_X -= 0.05f;
                if (isHitLeft && random_X + 0.05f <= 2) random_X += 0.05f;
            }
            if (isHitLeft && isHitRight && transform.position.x < hitLeft.transform.position.x) random_X = -2;
            if (isHitLeft && isHitRight && transform.position.x > hitLeft.transform.position.x) random_X = 2;

            rb.velocity = new Vector3(random_X, 0, Mathf.Sqrt(Mathf.Abs(2.2f * 2.2f - random_X * random_X)));
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            anim.SetInteger("state", 1);
        }
        else
        {
            rb.velocity = Vector3.zero;
            if (anim.GetInteger("state") != 16) anim.SetInteger("state", 0);
        }
    }
    void Destroy_BOT()
    {
        if (player_ctrl.time_value == 100)
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
        if (player_ctrl.time_value == 100 && rb.velocity.magnitude > 0.001f && !safe)
        {
            show_laze();

            alert.SetActive(true);
            Invoke("alert_active_false", 0.5f);

            end_game = true;

            anim.SetInteger("state", 16);
            Invoke("gameobject_setactive_false", 3f);
            //Ragdoll ragdollScript = gameObject.GetComponent<Ragdoll>();
            //if (ragdollScript != null)
            //{
            //    ragdollScript.SetRagdollState(true);
            //}
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
        laze.SetPosition(1, new Vector3(0, 5, 55));
        Invoke("hide_laze", 0.25f);
    }
    void hide_laze()
    {
        laze.gameObject.SetActive(false);
    }
    void random_randomX()
    {
        if (!isHitLeft && !isHitRight) myTweener = DOTween.To(() => random_X, x => random_X = x, Random.Range(-1f, 1f), 1);
        Invoke("random_randomX", 1);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("safe"))
    //    {
    //        safe = true;
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("safe"))
        {
            Invoke("random_randomX", 0.3f);
        }
        if (other.CompareTag("Finish"))
        {
            end_game = true;
            player_ctrl.place_value++;
        }
    }
    void gameobject_setactive_false()
    {
        gameObject.SetActive(false);
    }
}
