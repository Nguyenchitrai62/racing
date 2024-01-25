using DG.Tweening;
using UnityEngine;

public class Scale_up : MonoBehaviour
{
    public float scale_ratio = 2;
    public float duration = 1.0f;

    private void OnEnable()
    {
        ScaleUp();
    }

    void ScaleUp()
    {
        transform.DOScale(transform.localScale * scale_ratio, duration).OnComplete(() => gameObject.SetActive(false));
    }
}
