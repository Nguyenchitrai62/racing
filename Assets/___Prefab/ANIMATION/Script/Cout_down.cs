using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cout_down : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float duration = 3;
    public List<GameObject> set_active_true;

    private float value;
    private void OnEnable()
    {
        value = duration;
        run();
    }
    void run()
    {
        if (value > 0)
        {
            if (value != 0) text.text = value.ToString();
            value--;
            Invoke("run", 1);
        }
        else
        {
            foreach (GameObject obj in set_active_true) obj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}