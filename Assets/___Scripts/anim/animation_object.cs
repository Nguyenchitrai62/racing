using DG.Tweening;
using UnityEngine;

public class animation_object : MonoBehaviour
{
    public float scaleMultiplier = 1.2f;
    public float animationDuration = 0.5f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    private void Start()
    {
        // Lưu scale ban đầu của GameObject
        initialScale = transform.localScale;

        // Tính toán scale mục tiêu
        targetScale = initialScale * scaleMultiplier;

        // Bắt đầu animation liên tục sử dụng DOTween
        AnimateScale();
    }

    private void AnimateScale()
    {
        transform.DOScale(targetScale, animationDuration)
            .OnComplete(() =>
            {
                transform.DOScale(initialScale, animationDuration)
                    .OnComplete(() =>
                    {
                        AnimateScale();  // Gọi lại khi hoàn thành để tạo animation liên tục
                    });
            });
    }
}
