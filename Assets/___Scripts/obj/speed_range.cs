using UnityEngine;
using UnityEngine.UI;

public class speed_range : MonoBehaviour
{
    public GameObject speed_bt;
    public GameObject speed_bt_ad;
    public GameObject speed_bt_max;

    public GameObject range_bt;
    public GameObject range_bt_ad;
    public GameObject range_bt_max;

    private void Update()
    {
        if (canvas_controller.Instance.main_menu)
        {
            float speed_temp = Mathf.RoundToInt((canvas_controller.speed - 4.7f) / 0.3f);
            if (canvas_controller.money >= speed_temp * 100)
            {
                speed_bt.SetActive(true);
                speed_bt_ad.SetActive(false);
            }
            else
            {
                speed_bt.SetActive(false);
                speed_bt_ad.SetActive(true);
            }
            if (canvas_controller.money >= canvas_controller.range * 100)
            {
                range_bt.SetActive(true);
                range_bt_ad.SetActive(false);
            }
            else
            {
                range_bt.SetActive(false);
                range_bt_ad.SetActive(true);
            }
            if (speed_temp == 10)
            {
                speed_bt_max.SetActive(true);
                speed_bt_max.transform.parent.GetComponent<Button>().enabled = false;
            }
            if (canvas_controller.range == 10)
            {
                range_bt_max.SetActive(true);
                range_bt_max.transform.parent.GetComponent<Button>().enabled = false;
            }
        }

    }
}
