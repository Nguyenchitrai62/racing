using TMPro;
using UnityEngine;

public class draw : MonoBehaviour
{
    public float radius = 1.0f;
    public float lineWidth = 0.2f;
    public float cost = 500;
    public TextMeshPro text;
    public GameObject obj_to_show;

    private LineRenderer line;
    public static bool check;
    private int i = 0;
    private float start_time;
    private float cur_cost;
    private string name_obj;

    private void OnDisable()
    {
        check = false;

    }
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        name_obj = gameObject.name;
        cur_cost = PlayerPrefs.GetFloat(name_obj, cost);

        text.text = cur_cost.ToString();
        i = Mathf.RoundToInt(360 - (cur_cost / cost) * 360);
        DrawArc(i);

        obj_to_show.SetActive(false);
    }
    private void FixedUpdate()
    {

        if (check && Time.time - start_time > 1 && cur_cost > 0)
        {
            if (canvas_controller.money > Mathf.RoundToInt(cost / 100) && cur_cost > Mathf.RoundToInt(cost / 100))
            {
                canvas_controller.money -= Mathf.RoundToInt(cost / 100);
                PlayerPrefs.SetFloat("money", canvas_controller.money);

                cur_cost -= Mathf.RoundToInt(cost / 100);
                PlayerPrefs.SetFloat(name_obj, cur_cost);

                text.text = cur_cost.ToString();
                i = Mathf.RoundToInt(360 - (cur_cost / cost) * 360);
                DrawArc(i);
            }
            else
            {
                if (canvas_controller.money < cur_cost)
                {
                    cur_cost -= canvas_controller.money;
                    PlayerPrefs.SetFloat(name_obj, cur_cost);

                    canvas_controller.money = 0;
                    PlayerPrefs.SetFloat("money", canvas_controller.money);

                    text.text = cur_cost.ToString();
                    i = Mathf.RoundToInt(360 - (cur_cost / cost) * 360);
                    DrawArc(i);
                }
                else
                {
                    canvas_controller.money -= cur_cost;
                    PlayerPrefs.SetFloat("money", canvas_controller.money);

                    cur_cost = 0;
                    PlayerPrefs.SetFloat(name_obj, cur_cost);
                }
            }
        }
        if (cur_cost == 0)
        {
            gameObject.SetActive(false);
            obj_to_show.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            check = true;
            start_time = Time.time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            check = false;
        }
    }

    public void DrawArc(int angle)
    {
        line.positionCount = angle + 1;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;


        for (int i = 0; i <= angle; i++)
        {
            float angleInRadians = i * Mathf.Deg2Rad;
            float x = Mathf.Sin(angleInRadians) * radius;
            float y = Mathf.Cos(angleInRadians) * radius;
            line.SetPosition(i, new Vector3(x + transform.position.x, transform.position.y + lineWidth / 2, y + transform.position.z));
        }
    }
}
