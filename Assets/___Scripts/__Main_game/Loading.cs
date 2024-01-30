using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    public Image progressBar;
    public float timer;
    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
        timer = Time.time;
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("game_1");

        // Đảm bảo cảnh không tự động kích hoạt khi tải xong
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // Cập nhật UI progressBar
            if ((Time.time - timer) / 2f < 1)
            progressBar.fillAmount = /*asyncLoad.progress*/ (Time.time - timer) / 2f;

            // Kích hoạt cảnh khi tải xong (progress >= 0.9 do Unity giữ lại 10% cuối cùng)
            if (asyncLoad.progress >= 0.9f && progressBar.fillAmount > 0.95f)
            {
                // Kích hoạt cảnh tải xong
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
