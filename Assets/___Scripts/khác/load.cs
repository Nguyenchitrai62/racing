using UnityEngine;
using UnityEngine.SceneManagement;

public class load : MonoBehaviour
{
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    public void play_mini_game()
    {
        canvas_controller.Instance.main_menu = false;
        canvas_controller.Instance.all_minigame.SetActive(false);
        canvas_controller.Instance.mini_game = true;
        canvas_controller.Instance._time_minigame = Time.time;
        canvas_controller.Instance.UI_start.SetActive(true);
        //Invoke("time_scale_0", 0.02f);
    }
    void time_scale_0()
    {
        Time.timeScale = 0;
    }
    public void load_whack_a_mouse()
    {
        SceneManager.LoadScene("game_1");
        Invoke("play_mini_game", 0.05f);
    }
    public void load_whack_a_mouse_2()
    {
        Invoke("load_whack_a_mouse", 1.5f);
    }
}
