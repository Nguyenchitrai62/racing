using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Object_prefab;
    public int pooledAmount = 10;
    public bool willGrow = true;
    private List<GameObject> pooledObjects;

    public List<GameObject> map;
    private float cur_obj_position = 17;
    private float old_position_y = 10;
    private float distance_obj_spawn = 7;

    void Awake()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(Object_prefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        Invoke("hide_obj", 1.5f);
        Invoke("check_spawn", 1);
    }

    private void Update()
    {


    }
    void check_spawn()
    {
        if (cur_obj_position - transform.position.z < 20)
        {
            float random_temp = Random.Range(-2, 3);
            if (old_position_y > 15) random_temp = Random.Range(-2, 1);
            else if (old_position_y < 5) random_temp = Random.Range(0, 3);
            old_position_y += random_temp;

            GameObject temp = GetPooledObject();
            temp.SetActive(true);
            temp.transform.position = new Vector3(0, old_position_y, cur_obj_position);
            cur_obj_position += distance_obj_spawn;
        }
        foreach (GameObject obj in map)
        {
            if (transform.position.z - obj.transform.position.z > 70)
            {
                obj.transform.position += new Vector3(0, 0, 144);
            }
        }
        Invoke("check_spawn", 1);
    }
    public void hide_obj()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeSelf && transform.position.z - obj.transform.position.z > 10)
            {
                obj.SetActive(false);
            }
        }
        Invoke("hide_obj", 2f);
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(Object_prefab);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
