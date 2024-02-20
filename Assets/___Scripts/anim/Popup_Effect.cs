using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PopupEffect : MonoBehaviour
{
    public Vector3 targetScale = Vector3.one; // Scale mục tiêu (mặc định là (1, 1, 1))
    private Vector3 initialScale; // Scale ban đầu

    void OnEnable()
    {
        initialScale = transform.localScale; // Lưu scale ban đầu của game object
        transform.localScale = Vector3.zero; // Thiết lập scale ban đầu là 0 để bắt đầu hiệu ứng từ scale 0

        // Tạo hiệu ứng popup bằng DOTween
        transform.DOScale(targetScale, 0.5f)
            .SetEase(Ease.OutBack) // Sử dụng ease-out back để tạo hiệu ứng nhấp nháy
            .SetUpdate(true)
            .OnComplete(() => transform.localScale = targetScale); // Đảm bảo rằng scale cuối cùng là scale mục tiêu
    }
    public void close_effect(GameObject obj)
    {
        // Tạo hiệu ứng thu nhỏ bằng DOTween khi game object bị vô hiệu hóa
        transform.DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InBack) // Sử dụng ease-in back để tạo hiệu ứng nhấp nháy khi thu nhỏ
            .SetUpdate(true)
            .OnComplete(() => transform.localScale = Vector3.zero)
            .OnComplete(() => obj.SetActive(false));
    }
}
