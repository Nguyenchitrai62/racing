using UnityEngine;
using UnityEngine.SceneManagement;

public class quay_ngang : MonoBehaviour
{
    // Sử dụng hàm này cho việc khởi tạo
    void Start()
    {
        // Thiết lập chế độ màn hình ngang khi scene được tải
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        // Hoặc bạn có thể sử dụng ScreenOrientation.LandscapeRight tùy thuộc vào yêu cầu của bạn
    }

    // Nếu bạn muốn thay đổi hướng màn hình dựa trên việc tải scenes
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kiểm tra xem scene hiện tại có phải là scene bạn muốn thiết lập chế độ màn hình ngang không
        if (scene.name == "kart") // thay thế "YourSceneNameHere" bằng tên thực sự của scene bạn muốn thay đổi
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            // Hoặc ScreenOrientation.LandscapeRight
        }
    }
}
