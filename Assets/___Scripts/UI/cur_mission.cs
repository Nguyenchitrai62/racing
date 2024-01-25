using UnityEngine;
using UnityEngine.UI;

public class cur_mission : MonoBehaviour
{
    public Image image_1;
    public Image image_2;
    public Image image_3;
    public Sprite[] images;
    private void Update()
    {
        if (canvas_controller.max_score == 1)
        {
            image_2.gameObject.SetActive(false);
            image_3.gameObject.SetActive(false);
        }
        if (canvas_controller.max_score == 2)
        {
            image_3.gameObject.SetActive(false);
        }
        if (canvas_controller.cur_score == 0)
        {
            image_1.sprite = images[0];
            image_2.sprite = images[0];
            image_3.sprite = images[0];
        }
        if (canvas_controller.cur_score == 1)
        {
            image_1.sprite = images[1];
            image_2.sprite = images[0];
            image_3.sprite = images[0];
        }
        if (canvas_controller.cur_score == 2)
        {
            image_1.sprite = images[1];
            image_2.sprite = images[1];
            image_3.sprite = images[0];
        }
        if (canvas_controller.cur_score == 3)
        {
            image_1.sprite = images[1];
            image_2.sprite = images[1];
            image_3.sprite = images[1];
        }
    }
}
