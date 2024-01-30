using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class enemy_controller : MonoBehaviour
{
    private float speed = 1f;
    public float raycastDistance = 3f;
    public float timer = 0;
    public MeshRenderer hinh_non;

    public float stun = 0;
    private Vector3 targetPosition;
    private Animator anim;
    private bool have_target = false;
    private bool target_trc = false;
    private Rigidbody rb;

    private NavMeshAgent nav;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        stun = 0;
    }
    private void Start()
    {
        transform.Find("SmokeWhiteSoftTrail").gameObject.SetActive(true);
        transform.Find("Hinh_non").gameObject.SetActive(true);
    }
    void Update()
    {
        if (canvas_controller.Instance.play && canvas_controller.cur_score < canvas_controller.max_score)
        {
            if (nav.velocity.magnitude == 0f /*Vector3.Distance(targetPosition, transform.position) < 0.1f*/)
            {
                if (timer < 0) anim.SetInteger("state", 0);
                else anim.SetInteger("state", 15);
            }
            else anim.SetInteger("state", 1);
            stun -= Time.deltaTime;
            if (stun <= 0)
            {
                if (timer < 0)
                {
                    Vector3 huong_nhin = targetPosition - transform.position;
                    huong_nhin.y = 0;
                    transform.rotation = Quaternion.LookRotation(huong_nhin);
                }

                rb.isKinematic = false;
                TargetPlayer();
                // đang target player thì mất target
                if (target_trc && !have_target)
                {
                    targetPosition = transform.position;
                    timer = 2f;

                }
                target_trc = have_target;

                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    //SetNewRandomTarget();
                    SetRandomDestination();
                    timer = 4f;
                }
                //Vector3 huong = targetPosition - transform.position;
                //huong.Normalize();
                //rb.velocity = huong * speed;
            }
            else
            {
                hinh_non.gameObject.SetActive(false);
                rb.isKinematic = true;
                anim.SetInteger("state", 14);
            }
        }
    }

    void SetNewRandomTarget()
    {
        float randomX;
        float randomZ;
        if (Random.value < 0.5f) randomX = Random.Range(-10f, -5f);
        else randomX = Random.Range(5f, 10f);
        if (Random.value < 0.5f) randomZ = Random.Range(-10f, -5f);
        else randomZ = Random.Range(5f, 10f);

        targetPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        Vector3 huong_nhin = targetPosition - transform.position;
        huong_nhin.y = 0;
        Quaternion lookOnLook = Quaternion.LookRotation(huong_nhin);
        transform.DORotateQuaternion(lookOnLook, 1f).SetEase(Ease.InOutSine);

        //transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    void TargetPlayer()
    {
        nav.speed = 2.5f;

        hinh_non.gameObject.SetActive(true);
        hinh_non.material.color = Color.green;

        have_target = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.tag == "player" || hit.transform.tag == "BOT" && Vector3.Distance(hit.transform.position, transform.position) < 3)
            {
                have_target = true;

                nav.SetDestination(hit.transform.position);
                targetPosition = hit.transform.position + new Vector3(0, 0.1f, 0);

                Vector3 huong_nhin = targetPosition - transform.position;
                huong_nhin.y = 0;
                transform.rotation = Quaternion.LookRotation(huong_nhin);

                nav.speed = 3.5f;
                timer = 4f;

                hinh_non.material.color = Color.yellow;
            }
            if (hit.transform.tag == "block" && Vector3.Distance(hit.point, transform.position) < 1)
            {
                targetPosition = transform.position;
                timer = 0;
            }
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, -30f, 0) * transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.tag == "player" || hit.transform.tag == "BOT" && Vector3.Distance(hit.transform.position, transform.position) < 3)
            {
                have_target = true;

                nav.SetDestination(hit.transform.position);
                targetPosition = hit.transform.position + new Vector3(0, 0.1f, 0);

                Vector3 huong_nhin = targetPosition - transform.position;
                huong_nhin.y = 0;
                transform.rotation = Quaternion.LookRotation(huong_nhin);

                nav.speed = 3.5f;
                timer = 4f;

                hinh_non.material.color = Color.yellow;
            }
            if (hit.transform.tag == "block" && Vector3.Distance(hit.point, transform.position) < 1)
            {
                targetPosition = transform.position;
                timer = 0;
            }
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 30f, 0) * transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.tag == "player" || hit.transform.tag == "BOT" && Vector3.Distance(hit.transform.position, transform.position) < 3)
            {
                have_target = true;

                nav.SetDestination(hit.transform.position);
                targetPosition = hit.transform.position + new Vector3(0, 0.1f, 0);

                Vector3 huong_nhin = targetPosition - transform.position;
                huong_nhin.y = 0;
                transform.rotation = Quaternion.LookRotation(huong_nhin);

                nav.speed = 3.5f;
                timer = 4f;

                hinh_non.material.color = Color.yellow;
            }
            if (hit.transform.tag == "block" && Vector3.Distance(hit.point, transform.position) < 1)
            {
                targetPosition = transform.position;
                timer = 0;
            }
        }
        if (have_target)
        {
            anim.SetInteger("state", 12);
            //Sound_Manager.Instance.Play_sound_effect(49);
            //Sound_Manager.Instance.Stop_sound_effect(30);
        }
        else
        {
            //Sound_Manager.Instance.Play_sound_effect(30);
            //Sound_Manager.Instance.Stop_sound_effect(49);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fire"))
        {
            nav.SetDestination(transform.position);
            Sound_Manager.Instance.Play_Sound("Stun");
            stun = 4f;
            timer = 0;
            transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
        }
        if (other.CompareTag("player") && stun < 0)
        {
            hinh_non.gameObject.SetActive(false);
            targetPosition = other.transform.position;

            Vector3 huong_nhin = targetPosition - transform.position;
            huong_nhin.y = 0;
            transform.rotation = Quaternion.LookRotation(huong_nhin);

            rb.isKinematic = true;
            canvas_controller.Instance.play = false;
            Invoke("gameover_state", 2f);
            anim.SetInteger("state", 13);
        }
        if (other.CompareTag("BOT"))
        {
            SetRandomDestination();
        }
    }
    private void gameover_state()
    {
        canvas_controller.Instance.game_over = true;
    }







    void SetRandomDestination()
    {
        Vector3 randomPoint = GetRandomPointInNavMesh();
        nav.SetDestination(randomPoint);
    }

    Vector3 GetRandomPointInNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int randomIndex = Random.Range(0, navMeshData.vertices.Length);
        Vector3 randomPoint = navMeshData.vertices[randomIndex];

        return randomPoint;
    }

}
