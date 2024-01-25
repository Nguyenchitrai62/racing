using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Red_light_effect : MonoBehaviour
{
    public float blinkTime = 0.5f; // Thời gian để hiệu ứng nháy đỏ diễn ra

    private Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        // Đặt màu ban đầu của Image là màu đỏ với độ trong suốt là 0
        imageComponent.color = new Color(1, 0, 0, 0);
    }

    private void OnEnable()
    {
        // Thay đổi độ trong suốt của Image từ 0 đến 1 và quay lại 0
        imageComponent.DOFade(1, blinkTime / 2).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            // Set Image này thành inactive sau khi hiệu ứng hoàn tất
            gameObject.SetActive(false);
        });
    }
}
