using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class canvas_shotter : MonoBehaviour
{
    public GameObject UI_start;
    public GameObject UI_game_over;

    public TextMeshProUGUI score_text;
    public TextMeshProUGUI money_value;
    public TextMeshProUGUI score_value_text;
    public TextMeshProUGUI money_claim_value;

    public Button myButton;

    private bool check;

    private float money_claim_x1;

    public TextMeshProUGUI claim_x1_text;
    public TextMeshProUGUI claim_x2_text;

    private void Awake()
    {
        Time.timeScale = 0;
        UI_start.SetActive(true);
    }
    private void OnEnable()
    {
        check = true;
    }
    private void Update()
    {
        money_value.text = canvas_controller.money.ToString();
        if (check && UI_game_over.activeSelf)
        {
            claim_x1_text.text = (Bullet.score * 5).ToString();
            claim_x2_text.text = (Bullet.score * 10).ToString();

            score_value_text.text = Bullet.score.ToString();
            money_claim_value.text = (Bullet.score * 5).ToString();
            money_claim_x1 = Bullet.score * 5;
            check = false;
            if (Bullet.score == 0) myButton.onClick.Invoke();
        }
    }
    public void plus_10()
    {
        canvas_controller.money += money_claim_x1;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    public void plus_10x2()
    {
        canvas_controller.money += money_claim_x1 * 2;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    public void claim()
    {
        Invoke("plus_10", 1.2f);
        Invoke("load_scene", 1.5f);
    }
    public void claimx2()
    {
        money_claim_value.text = (Bullet.score * 10).ToString();
        Invoke("plus_10x2", 1.2f);
        Invoke("load_scene", 1.5f);
    }
    public void back()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("game_1");
    }
    public void start_game()
    {
        Time.timeScale = 1;
        UI_start.SetActive(false);
        Bullet.score = 0;
    }
    public void load_scene()
    {
        SceneManager.LoadScene("shotter");
    }
    public void restart()
    {
        SceneManager.LoadScene("game_1");
    }
}
