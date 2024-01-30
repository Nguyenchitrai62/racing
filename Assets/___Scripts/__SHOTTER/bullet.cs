using DG.Tweening;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject gift;
    public GameObject rocket_bt;

    public TextMeshProUGUI score_text;

    public static int rocket;
    public static int score;
    public static float time_to_destroy_gift;

    public ParticleSystem vfx; // item_box_claim
    public float crit_rate = 25;
    public float drop_rate = 20;

    public GameObject _no;
    public static bool shake_camera = false;
    private void Awake()
    {
        rocket = 0;
        score = 0;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, mainCamera.transform.position) > 300)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (Scope.aim_enemy)
        {
            rocket_bt.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (rocket == 1)
        {
            rocket_bt.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (other.GetComponent<Animator>().GetInteger("state") != 16)
            {
                Sound_Manager.Instance.Play_Sound("Explosion");
                gameObject.SetActive(false);

                _no.transform.position = transform.position - new Vector3(0, 0, 3);
                _no.SetActive(true);

                //nếu còn 1 enemy cuối
                if (count_obj_active_true() == 1)
                {
                    spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] -= Scope.HP_Sub;
                }
                else
                {
                    //random tỉ lệ crit
                    int crit = Random.Range(0, 100);
                    if ((crit < crit_rate) || (crit < crit_rate * 2 && transform.position.y > 5f))
                    {
                        spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] -= 3;
                    }
                    else
                    {
                        spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)]--;
                    }
                }

                Transform quad_1 = other.transform.Find("quad_1");
                Transform quad_2 = other.transform.Find("quad_2");
                //ẩn enemy khi hết máu
                if (spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] <= 0)
                {
                    quad_1.gameObject.SetActive(false);
                    quad_2.gameObject.SetActive(false);
                    //other.gameObject.SetActive(false);

                    //Ragdoll ragdollScript = other.GetComponent<Ragdoll>();
                    //if (ragdollScript != null)
                    //{
                    //    // Kích hoạt ragdoll thay vì làm enemy vô hiệu.
                    //    ragdollScript.SetRagdollState(true);
                    //}

                    other.GetComponent<Animator>().SetInteger("state", 16);
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    DOTween.Sequence()
                    .AppendInterval(3f)
                    .OnComplete(() => gameobject_setactive_false(other.gameObject));

                    if (count_obj_active_true() == 0)
                    {
                        CancelInvoke("new_wave");
                        Invoke("new_wave", 1.5f);
                    }

                    score++;
                    score_text.text = score.ToString();

                    //random quà khi tiêu diệt
                    int buff = Random.Range(0, 100);
                    if (buff < drop_rate && !gift.activeSelf && rocket == 0)
                    {
                        gift.transform.position = other.transform.position + new Vector3(0, 10, 0);
                        gift.SetActive(true);
                        time_to_destroy_gift = Time.time;
                    }
                }
                else if (spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] < 3) //xử lí hiệu ứng thanh máu
                {
                    quad_1.gameObject.SetActive(true);
                    quad_2.gameObject.SetActive(true);
                    Vector3 newScale = quad_2.localScale;
                    newScale.x = spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] * 0.2f;
                    quad_2.localScale = newScale;
                    quad_2.localPosition = quad_1.localPosition + new Vector3(((spawn.cur_HP[spawn.pooledObjects.IndexOf(other.gameObject)] - 3) * -0.1f), 0, quad_2.localPosition.z);
                }
            }

        }
        if (other.gameObject.CompareTag("buff"))
        {
            Sound_Manager.Instance.Play_Sound("BuffBox");

            vfx.transform.position = other.transform.position;
            vfx.gameObject.SetActive(true);

            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            rocket = 1;
            rocket_bt.SetActive(true);
        }

    }
    public void fire_rocket()
    {
        Sound_Manager.Instance.Play_Sound("Rocket Coming");

        rocket_bt.SetActive(false);
        rocket = 2;
        Invoke("kill_all_enemy", 2);
    }
    public void kill_all_enemy()
    {
        shake_camera = true;
        rocket = 0;
        foreach (GameObject obj in spawn.pooledObjects)
        {
            if (obj.activeSelf && obj.GetComponent<Animator>().GetInteger("state") != 16)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                obj.transform.Find("quad_1").gameObject.SetActive(false);
                obj.transform.Find("quad_2").gameObject.SetActive(false);

                //Ragdoll ragdollScript = obj.GetComponent<Ragdoll>();
                //if (ragdollScript != null)
                //{
                //    ragdollScript.SetRagdollState(true);
                //}

                obj.GetComponent<Animator>().SetInteger("state", 16);
                DOTween.Sequence()
                .AppendInterval(3f)
                .OnComplete(() => gameobject_setactive_false(obj));

                score++;
                score_text.text = score.ToString();
            }
        }
        CancelInvoke("new_wave");
        Invoke("new_wave", 2);
    }
    public void new_wave()
    {
        spawn.wave++;
        spawn.is_spawn = true;
    }
    public int count_obj_active_true()
    {
        int count_obj_active_true = 0;
        foreach (GameObject obj in spawn.pooledObjects)
        {
            if (obj.activeSelf && obj.GetComponent<Animator>().GetInteger("state") != 16)
            {
                count_obj_active_true++;
            }
        }
        return count_obj_active_true;
    }
    void gameobject_setactive_false(GameObject obj)
    {
        obj.SetActive(false);
    }
}
