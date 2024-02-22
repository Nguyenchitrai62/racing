using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    public Slider progressBar;
    public float timer;

    public GameObject Scene_Transitions;
    private float time_active_scene;
    public static bool play_Scene_Transitions;
    bool check = true;

    void Start()
    {
        Application.targetFrameRate = 120;

        play_Scene_Transitions = true;
        //StartCoroutine(LoadMainSceneAsync());
        timer = Time.time;
    }
    private void Update()
    {
        if ((Time.time - timer) / 2f < 1)
        {
            progressBar.value = (Time.time - timer) / 2f;
            time_active_scene = Time.time;
        }
        else
        {
            if (!Scene_Transitions.activeSelf)
            {
                Scene_Transitions.SetActive(true);
                Scene_Transitions.GetComponent<Animator>().SetInteger("state", 1);
            }
            if (Time.time - time_active_scene > 0.5f && check)
            {
                check = false;
                SceneManager.LoadSceneAsync("game_1");
            }
        }
    }

}
