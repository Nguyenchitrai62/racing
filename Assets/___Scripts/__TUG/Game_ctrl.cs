using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_ctrl : MonoBehaviour
{
    public Image tren_green;
    public Image tren_red;
    public GameObject target;

    public float sub_per_second = 0.2f;
    public float plus_per_tap = 0.2f;

    public float point = 20;
    public GameObject All_char;

    public GameObject game_over;
    public GameObject game_ctrl;

    private bool check = true;

    public Button myButton;
    public TextMeshProUGUI monney_value_text;
    public TextMeshProUGUI monney_claim_value;

    private bool freezer;
    private bool strong_pull;

    public GameObject node;

    public static float level = 0;

    public List<GameObject> player;

    private void Awake()
    {
        Sound_Manager.Instance.Play_Music("tug", 0);
        check = true;
        canvas_controller.money = PlayerPrefs.GetFloat("money", 0);
        monney_value_text.text = canvas_controller.money.ToString();

        freezer = false;
        strong_pull = false;
    }
    private void FixedUpdate()
    {
        if (check && game_ctrl.activeSelf && !game_over.activeSelf)
        {
            if (player[0].GetComponent<Animator>().GetInteger("state") == 0)
            {
                for (int i = 0; i < player.Count; i++)
                {
                    player[i].GetComponent<Animator>().SetInteger("state", 8);
                }
            }


            Sound_Manager.Instance.Play_Music("tug", 1);
            sub();
            random_target();
            check_process();
            check = false;
        }
        node.transform.localPosition = new Vector3(node.transform.localPosition.x, 900 * tren_green.fillAmount - 450, node.transform.localPosition.z);
        monney_value_text.text = canvas_controller.money.ToString();
    }
    void sub()
    {
        tren_green.fillAmount -= sub_per_second / 20;
        tren_red.fillAmount = tren_green.fillAmount;
        Invoke("sub", 0.05f);
    }
    public void plus()
    {
        tren_green.fillAmount += plus_per_tap;
        tren_red.fillAmount = tren_green.fillAmount;
    }
    void random_target()
    {
        float randomX = Random.Range(-300f, 300f);

        Vector3 newLocalPosition = target.transform.localPosition;
        newLocalPosition.y = randomX;

        target.transform.DOLocalMove(newLocalPosition, 1.0f).SetEase(Ease.Linear);
        Invoke("random_target", 3f);
    }
    void check_process()
    {
        if (tren_green.fillAmount * 800 - 400 < target.transform.localPosition.y + 100 &&
            tren_green.fillAmount * 800 - 400 > target.transform.localPosition.y - 100)
        {
            tren_green.gameObject.SetActive(true);

            point++;
            if (strong_pull) point++;

            if (point > 200)
            {
                Sound_Manager.Instance.Play_Music("tug", 0);
                Sound_Manager.Instance.Play_Sound("Wilhelm");
                Invoke("play_game_win", 1);

                CancelInvoke("sub");
                CancelInvoke("random_target");

                game_over.SetActive(true);
                game_ctrl.SetActive(false);
                monney_claim_value.text = "100";
                level++;
            }
        }
        else
        {
            tren_green.gameObject.SetActive(false);

            if (!freezer) point -= 1 + level * 0.25f;

            if (point < -200)
            {
                Sound_Manager.Instance.Play_Music("tug", 0);
                Sound_Manager.Instance.Play_Sound("Wilhelm");
                Invoke("play_game_lost", 1);

                CancelInvoke("sub");
                CancelInvoke("random_target");

                myButton.onClick.Invoke();
                game_over.SetActive(true);
                game_ctrl.SetActive(false);
                level = 0;
            }
        }
        All_char.transform.position = new Vector3(All_char.transform.position.x, All_char.transform.position.y, -point / 25f);
        if (!game_over.activeSelf) Invoke("check_process", 0.05f);
    }

    public void replay()
    {
        SceneManager.LoadScene("tug");
    }
    public void back()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("game_1");
    }
    public void claim_x1()
    {
        Invoke("plus_money_x1", 1.2f);
        Invoke("replay", 1.5f);
    }
    public void claim_x2()
    {
        monney_claim_value.text = "200";
        Invoke("plus_money_x2", 1.2f);
        Invoke("replay", 1.5f);
    }
    public void plus_money_x1()
    {
        canvas_controller.money += 100;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    void plus_money_x2()
    {
        canvas_controller.money += 200;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }

    public void Freezer()
    {
        Sound_Manager.Instance.Play_Sound("IceBuff");
        freezer = true;
    }
    public void Strong_Pull()
    {
        Sound_Manager.Instance.Play_Sound("StrengthUp");
        strong_pull = true;
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
