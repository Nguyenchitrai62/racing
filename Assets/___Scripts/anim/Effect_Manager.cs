using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Manager : MonoBehaviour
{
    public static Effect_Manager Instance;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start_white_flash()
    {
        image.DOFade(0.75f, 0.1f).OnComplete(() =>
        {
            image.DOFade(0f, 0.3f);
        });
    }

}
