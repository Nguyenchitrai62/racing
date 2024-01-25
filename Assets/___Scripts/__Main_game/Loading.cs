using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    public Image progressBar;

    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("game_1");

        // Đảm bảo cảnh không tự động kích hoạt khi tải xong
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // Cập nhật UI progressBar
            progressBar.fillAmount = asyncLoad.progress;

            // Kích hoạt cảnh khi tải xong (progress >= 0.9 do Unity giữ lại 10% cuối cùng)
            if (asyncLoad.progress >= 0.9f)
            {
                // Kích hoạt cảnh tải xong
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
