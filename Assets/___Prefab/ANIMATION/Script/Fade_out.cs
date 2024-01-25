using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fade_out : MonoBehaviour
{
    private Image image;
    public float Duration = 1.0f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Color currentColor = image.color;
        currentColor.a = 0;
        image.color = currentColor;
        FadeOut();
    }

    void FadeOut()
    {
        image.DOFade(1, Duration).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() => gameObject.SetActive(false));
    }
}
