using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Green_Flash : MonoBehaviour
{
    private Image flashPanel;
    public float flashDuration = 0.5f;
    public float maxAlpha = 1f;
    private Tween flashTween;

    void Start()
    {
        flashPanel = GetComponent<Image>();
        flashPanel.color = new Color(flashPanel.color.r, flashPanel.color.g, flashPanel.color.b, 0);
    }

    void Update()
    {
        if (player_ctrl.time_value > 0 && player_ctrl.time_value < 10)
        {
            if (flashTween == null || !flashTween.IsActive() || !flashTween.IsPlaying())
            {
                StartFlashing();
            }
        }
        else
        {
            if (flashTween != null && flashTween.IsPlaying())
            {
                StopFlashing();
            }
        }
    }

    private void StartFlashing()
    {
        flashTween = flashPanel.DOFade(maxAlpha, flashDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutFlash);
    }

    private void StopFlashing()
    {
        flashTween.Kill();
        flashPanel.color = new Color(flashPanel.color.r, flashPanel.color.g, flashPanel.color.b, 0);
    }
}
