using System.Collections.Generic;
using UnityEngine;
// mini_game Home_map
public class show_item : MonoBehaviour
{
    public List<GameObject> item_to_show;
    private GameObject cost_obj_to_hide;//cost_
    private void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name.StartsWith("cost"))
            {
                cost_obj_to_hide = child.gameObject;
            }
        }
    }
    private void Update()
    {
        if (cost_obj_to_hide.activeSelf)
        {
            foreach (GameObject i in item_to_show)
            {
                i.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject i in item_to_show)
            {
                i.SetActive(true);
            }
        }
    }
}
