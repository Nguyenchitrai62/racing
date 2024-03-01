﻿using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomNavDestination : MonoBehaviour
{
    public GameObject UI_Help_Prefab;
    private GameObject UI_help;
    private Image fill_amount_Image;
    private TextMeshProUGUI Help_text;

    public GameObject player;
    public GameObject long_sat;
    public GameObject smoke;

    private NavMeshAgent nav;
    private Animator anim;
    private BoxCollider box;

    public bool check = false;
    private Rigidbody rb;
    float start_time;
    private bool run_1_time = true;
    void Start()
    {

        if (UI_help == null)
        {
            UI_help = Instantiate(UI_Help_Prefab, GameObject.Find("Canvas").transform.Find("play"));
            fill_amount_Image = UI_help.transform.Find("Fill_Amount").GetComponent<Image>();
            Help_text = UI_help.transform.Find("Text").GetComponent<TextMeshProUGUI>();   
        }
        UI_help.SetActive(false);
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
        box.enabled = false;
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = GetRandomPointInNavMesh();
        nav.SetDestination(randomPoint);
        Invoke("SetRandomDestination", Random.Range(3f, 5f));
    }

    Vector3 GetRandomPointInNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int randomIndex = Random.Range(0, navMeshData.vertices.Length);
        Vector3 randomPoint = navMeshData.vertices[randomIndex];

        return randomPoint;
    }

    void Update()
    {
        //nav.destination = player.transform.position;
        if (player_controler.cout_down)
        {
            if (run_1_time)
            {
                transform.position = new Vector3(0.07f, 0.1f, -7.28f);
                smoke.SetActive(true);
                run_1_time = false;
                transform.Find("SmokeWhiteSoftTrail").gameObject.SetActive(true);
                box.enabled = true;
                SetRandomDestination();
            }
        }
        if (nav.velocity.magnitude > 0f && !long_sat.activeSelf) anim.SetInteger("state", 1);
        else anim.SetInteger("state", 0);

        if (check && Time.time - start_time > 1.5f)
        {
            check = false;
            UI_help.SetActive(false);

            long_sat.SetActive(false);
            gameObject.tag = "BOT";
            SetRandomDestination();
        }
        if (UI_help.activeSelf)
        {
            UI_help.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, Screen.height / 8);
            fill_amount_Image.fillAmount = (Time.time - start_time) / 1.5f;
            Help_text.text = Mathf.Ceil((1.5f - Time.time + start_time) / 0.5f).ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            long_sat.SetActive(true);
            smoke.SetActive(true);

            gameObject.tag = "Untagged";
            CancelInvoke();
            nav.ResetPath();
            anim.SetInteger("state", 0);
        }
        if (other.CompareTag("player") && long_sat.activeSelf)
        {
            check = true;

            UI_help.SetActive(true);
            start_time = Time.time;
        }
        if (other.CompareTag("coin"))
        {
            other.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            check = false;
            UI_help.SetActive(false);
        }
    }
}
