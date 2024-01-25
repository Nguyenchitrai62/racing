using TMPro;
using UnityEngine;

public class show_name : MonoBehaviour
{
    public TextMeshProUGUI uiText; // Phần tử Text UI
    private float maxDistance = 60f;
    private float max_fontsize = 70;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        uiText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsObjectVisible(mainCamera, gameObject))
        {
            uiText.gameObject.SetActive(true);

            float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
            float percentage = Mathf.Clamp01(distanceToCamera / maxDistance);
            uiText.fontSize = max_fontsize * (1 - percentage);

            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
            uiText.transform.position = screenPos + new Vector3(0, Screen.height / 6 * (1 - percentage), 0);
        }
        else
        {
            uiText.gameObject.SetActive(false);
        }
    }

    bool IsObjectVisible(Camera camera, GameObject go)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(go.transform.position);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
    }
}
