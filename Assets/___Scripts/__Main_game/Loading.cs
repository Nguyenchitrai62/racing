using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    public Image progressBar;
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
        // Cập nhật UI progressBar
        if ((Time.time - timer) / 2f < 1)
        {
            progressBar.fillAmount = (Time.time - timer) / 2f;
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

    //IEnumerator LoadMainSceneAsync()
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("game_1");

    //    // Đảm bảo cảnh không tự động kích hoạt khi tải xong
    //    asyncLoad.allowSceneActivation = false;

    //    while (!asyncLoad.isDone)
    //    {
    //        // Cập nhật UI progressBar
    //        if ((Time.time - timer) / 2f < 1)
    //            progressBar.fillAmount = /*asyncLoad.progress*/ (Time.time - timer) / 2f;

    //        if (asyncLoad.progress >= 0.9f && progressBar.fillAmount > 0.95f)
    //        {
    //            if (!Scene_Transitions.activeSelf) Scene_Transitions.SetActive(true);
    //            if (Time.time - time_active_scene > 2)
    //            {
    //                asyncLoad.allowSceneActivation = true;
    //            }
    //        }
    //        else time_active_scene = Time.time;

    //        yield return null;
    //    }
    //}
}
