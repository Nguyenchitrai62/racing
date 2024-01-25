using System.Collections.Generic;
using UnityEngine;

public class spawn_obj : MonoBehaviour
{
    public List<GameObject> Object_prefab;
    public int pooledAmount = 80;
    public bool willGrow = true;
    public List<GameObject> map;

    private List<GameObject> pooledObjects;
    private float cur_obj_position = 20;
    private float old_position;

    void Awake()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(Object_prefab[Random.Range(0, 4)]);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        Invoke("hide_obj", 1f);
    }

    private void Update()
    {
        if (cur_obj_position - transform.position.z < 100)
        {
            float random_temp = Random.Range(1, 4);
            for (int i = 1; i < 4; i++)
            {
                GameObject temp = GetPooledObject();
                temp.SetActive(true);
                temp.transform.position = new Vector3(i * 3, 1, cur_obj_position);
                if (i == random_temp)
                {
                    temp.GetComponent<Renderer>().enabled = false;
                    temp.GetComponent<Collider>().isTrigger = true;
                }
            }
            cur_obj_position += 20;
        }

        foreach (GameObject obj in map)
        {
            if (transform.position.z - obj.transform.position.z > 35)
            {
                obj.transform.position += new Vector3(0, 0, 144);
            }
        }
    }
    public void hide_obj()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeSelf && transform.position.z - obj.transform.position.z > 20)
            {
                obj.SetActive(false);
                obj.GetComponent<Renderer>().enabled = true;
                obj.GetComponent<Collider>().isTrigger = false;
            }
        }
        Invoke("hide_obj", 1f);
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
            GameObject obj = Instantiate(Object_prefab[Random.Range(0, 4)]);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
