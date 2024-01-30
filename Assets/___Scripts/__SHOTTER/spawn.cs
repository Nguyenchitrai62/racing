using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class spawn : MonoBehaviour
{
    public GameObject prefabToPool; // Prefab bạn muốn tạo ra
    public GameObject gift;
    public GameObject UI_game_over;

    public List<GameObject> GAN;
    public List<GameObject> VUA;
    public List<GameObject> XA;

    public int initialPoolSize = 20; // Kích thước ban đầu của pool

    public static List<GameObject> pooledObjects; // Danh sách chứa các đối tượng trong pool
    public static int[] cur_HP = new int[100];
    public static int wave;
    public static bool is_spawn;
    public static float speed_enemy = 10;

    private float scale_enemy = 15;

    private void OnEnable()
    {
        wave = 0;
        is_spawn = true;
        UI_game_over.SetActive(false);
        Invoke("set_wave_1", 1);
    }
    void set_wave_1()
    {
        wave = 1;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObjectInPool();
            cur_HP[i] = 3;
        }
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    GameObject temp = GetPooledObject();
        //    temp.transform.position = spawn_gan();
        //    Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
        //    temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
        //}
        if (UI_game_over.activeSelf) Scope.start_time_shot = Time.time + 1000000f;
        if (Time.time - Bullet.time_to_destroy_gift > 10)
        {
            gift.SetActive(false);
        }

        if (is_spawn && wave >= 1 && wave <= 3)
        {
            is_spawn = false;
            for (int i = 1; i <= wave; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_xa();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
        }
        if (is_spawn && wave >= 4 && wave <= 6)
        {
            is_spawn = false;
            int random_spawn_xa = Random.Range(0, wave + 1);
            for (int i = 1; i <= random_spawn_xa; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_xa();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
            for (int i = 1; i <= wave - random_spawn_xa; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_vua();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
        }
        if (is_spawn && wave >= 7)
        {
            is_spawn = false;
            int random_spawn_gan = Random.Range(0, 3);
            int random_spawn_xa = Random.Range(0, wave - random_spawn_gan + 1);
            for (int i = 1; i <= random_spawn_gan; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_gan();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
            for (int i = 1; i <= random_spawn_xa; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_xa();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
            for (int i = 1; i <= wave - random_spawn_xa - random_spawn_gan; i++)
            {
                GameObject temp = GetPooledObject();

                temp.transform.localScale = new Vector3(0, 0, 0);
                temp.transform.DOScale(new Vector3(scale_enemy, scale_enemy, scale_enemy), 1f);

                temp.transform.position = spawn_vua();

                Vector3 target = new Vector3(transform.position.x, 0, transform.position.z);
                temp.GetComponent<Rigidbody>().velocity = (target - temp.transform.position).normalized * speed_enemy;
                temp.GetComponent<Animator>().SetInteger("state", 11);
                temp.GetComponent<BoxCollider>().enabled = true;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Animator>().SetInteger("state", 13);
            Invoke("UI_game_over_active_true", 1.5f);
            Invoke("sound_and_effect", 1f);
            Scope.is_hold = false;
        }
    }


    GameObject CreateNewObjectInPool()
    {
        GameObject obj = Instantiate(prefabToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                cur_HP[pooledObjects.IndexOf(obj)] = 3;
                Transform quad_1 = obj.transform.Find("quad_1");
                Transform quad_2 = obj.transform.Find("quad_2");
                quad_1.gameObject.SetActive(false);
                quad_2.gameObject.SetActive(false);
                return obj;
            }
        }
        return CreateNewObjectInPool();
    }

    public Vector3 spawn_xa()
    {
        return XA[Random.Range(0, XA.Count)].transform.position;
    }
    public Vector3 spawn_vua()
    {
        return VUA[Random.Range(0, VUA.Count)].transform.position;
    }
    public Vector3 spawn_gan()
    {
        return GAN[Random.Range(0, GAN.Count)].transform.position;
    }
    public void DisableAllAnimators()
    {
        foreach (GameObject obj in pooledObjects)
        {
            Animator animator = obj.GetComponent<Animator>();
            animator.enabled = false;
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }
    void sound_and_effect()
    {
        Effect_Manager.Instance.Start_white_flash();
        Sound_Manager.Instance.Play_Sound("ZombieAttack");
    }
    void UI_game_over_active_true()
    {
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
        Sound_Manager.Instance.Play_Sound("GameLost");

        PlayerPrefs.SetFloat("score" + gameObject.scene.name, Bullet.score);
        UI_game_over.SetActive(true);
        DisableAllAnimators();
    }
}
