using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fade_in : MonoBehaviour
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
        currentColor.a = 1;
        image.color = currentColor;
        FadeIn();
    }

    void FadeIn()
    {
        image.DOFade(0, Duration).SetUpdate(true).SetEase(Ease.InExpo).OnComplete(() => gameObject.SetActive(false));
    }
}
