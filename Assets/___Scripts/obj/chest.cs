using UnityEngine;

public class chest : MonoBehaviour
{
    public GameObject cube_yellow;
    public GameObject cube_pink;
    public GameObject ground_yellow;
    public GameObject ground_pink;
    public GameObject wall;
    void Update()
    {
        if (Vector3.Distance(cube_pink.transform.position, ground_pink.transform.position) < 1)
        {
            ground_pink.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            ground_pink.GetComponent<Renderer>().material.color = cube_pink.GetComponent<Renderer>().material.color;

        }
        if (Vector3.Distance(cube_yellow.transform.position, ground_yellow.transform.position) < 1)
        {
            ground_yellow.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            ground_yellow.GetComponent<Renderer>().material.color = cube_yellow.GetComponent<Renderer>().material.color;
        }


        if (Vector3.Distance(cube_pink.transform.position, ground_pink.transform.position) < 1 &&
            Vector3.Distance(cube_yellow.transform.position, ground_yellow.transform.position) < 1)
        {
            wall.SetActive(false);
        }
        else
        {
            wall.SetActive(true);
        }
    }
}
