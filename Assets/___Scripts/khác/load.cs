using DG.Tweening;
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
        DOTween.Sequence()
             .AppendInterval(0.5f)
             .AppendCallback(() => {
                 canvas_controller.Instance.main_menu = false;
                 canvas_controller.Instance.all_minigame.SetActive(false);
                 canvas_controller.Instance.mini_game = true;
                 canvas_controller.Instance._time_minigame = Time.time;
                 canvas_controller.Instance.UI_start.SetActive(true);
             });
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
