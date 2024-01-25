using UnityEngine;
using UnityEngine.SceneManagement;

public class canvas_racing : MonoBehaviour
{
    public GameObject UI_start;
    private void Awake()
    {
        Time.timeScale = 0;
        UI_start.SetActive(true);
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
    }
}
