using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Input_manager : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public float duration = 0.2f;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    private void FixedUpdate()
    {
        //vertical = Input.GetAxis("Vertical");
        //horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
    }

    public void UP()
    {
        DOTween.To(() => vertical, x => vertical = x, 1, duration).SetEase(Ease.InQuad);
    }

    public void DOWN()
    {
        DOTween.To(() => vertical, x => vertical = x, -1, duration).SetEase(Ease.InQuad);
    }

    public void LEFT()
    {
        DOTween.To(() => horizontal, x => horizontal = x, -1, duration).SetEase(Ease.InQuad);
    }

    public void RIGHT()
    {
        DOTween.To(() => horizontal, x => horizontal = x, 1, duration).SetEase(Ease.InQuad);
    }
    public void horizontal_zero()
    {
        DOTween.To(() => horizontal, x => horizontal = x, 0, duration);
    }
    public void vertical_zero()
    {
        DOTween.To(() => vertical, x => vertical = x, 0, duration);
    }



    public void back()
    {
        SceneManager.LoadSceneAsync("game_1");
        Screen.orientation = ScreenOrientation.Portrait;
        Time.timeScale = 1;
    }
}
