using UnityEngine;
using UnityEngine.UI;

public class cur_level : MonoBehaviour
{
    public Image level_1;
    public Image level_2;
    public Image level_3;
    public Image level_4;
    public Image level_5;
    public Image main_level;

    public Sprite[] images;

    public void Update()
    {
        if (canvas_controller.level % 5 == 1)
        {
            level_1.sprite = images[1];
            level_2.sprite = images[0];
            level_3.sprite = images[0];
            level_4.sprite = images[0];
            level_5.sprite = images[0];
            main_level.rectTransform.anchoredPosition = level_1.rectTransform.anchoredPosition + new Vector2(0, 50);
        }
        if (canvas_controller.level % 5 == 2)
        {
            level_1.sprite = images[2];
            level_2.sprite = images[1];
            level_3.sprite = images[0];
            level_4.sprite = images[0];
            level_5.sprite = images[0];
            main_level.rectTransform.anchoredPosition = level_2.rectTransform.anchoredPosition + new Vector2(0, 50);
        }
        if (canvas_controller.level % 5 == 3)
        {
            level_1.sprite = images[2];
            level_2.sprite = images[2];
            level_3.sprite = images[1];
            level_4.sprite = images[0];
            level_5.sprite = images[0];
            main_level.rectTransform.anchoredPosition = level_3.rectTransform.anchoredPosition + new Vector2(0, 50);
        }
        if (canvas_controller.level % 5 == 4)
        {
            level_1.sprite = images[2];
            level_2.sprite = images[2];
            level_3.sprite = images[2];
            level_4.sprite = images[1];
            level_5.sprite = images[0];
            main_level.rectTransform.anchoredPosition = level_4.rectTransform.anchoredPosition + new Vector2(0, 50);
        }
        if (canvas_controller.level % 5 == 0)
        {
            level_1.sprite = images[2];
            level_2.sprite = images[2];
            level_3.sprite = images[2];
            level_4.sprite = images[2];
            level_5.sprite = images[1];
            main_level.rectTransform.anchoredPosition = level_5.rectTransform.anchoredPosition + new Vector2(0, 50);
        }
    }
}
