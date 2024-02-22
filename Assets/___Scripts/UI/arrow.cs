using UnityEngine;

public class arrow : MonoBehaviour
{
    public GameObject arrowImagePrefab;

    private GameObject arrowImage;

    private Camera mainCamera;

    private bool check;

    private BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        mainCamera = Camera.main;

        if (arrowImage == null)
        {
            arrowImage = Instantiate(arrowImagePrefab, GameObject.Find("Canvas").transform.Find("play")); // Tạo đối tượng Image từ Prefab
            arrowImage.gameObject.SetActive(false);
        }
        check = true;
    }

    void Update()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool isOffScreen = screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;

        if (isOffScreen && check)
        {
            arrowImage.gameObject.SetActive(true);

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
            screenPosition.z = 0;

            float arrowPosX = screenPosition.x;
            float arrowPosY = screenPosition.y;

            if (screenPosition.x < Screen.width / 20)
            {
                arrowPosX = Screen.width / 20;
            }
            else if (screenPosition.x > Screen.width - Screen.width / 20)
            {
                arrowPosX = Screen.width - Screen.width / 20;
            }

            if (screenPosition.y < Screen.width / 20 + Screen.height / 12.5f)
            {
                arrowPosY = Screen.width / 20 + Screen.height / 12.5f;
            }
            else if (screenPosition.y > Screen.height - Screen.width / 20)
            {
                arrowPosY = Screen.height - Screen.width / 20;
            }

            arrowImage.transform.position = new Vector3(arrowPosX, arrowPosY, 0);

            Vector3 direction = screenPosition - arrowImage.transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrowImage.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            arrowImage.gameObject.SetActive(false);
        }
        if (!box.enabled)
        {
            arrowImage.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            check = false;
        }
    }
    private void OnDisable()
    {
        arrowImage.gameObject.SetActive(false);
    }
}
