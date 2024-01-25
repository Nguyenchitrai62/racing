using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject vfx_prefab;
    private GameObject vfx;
    private void Awake()
    {
        vfx = Instantiate(vfx_prefab);
        vfx.transform.position = transform.position;
    }
    private void OnDisable()
    {
        Invoke("active_true", 2);
        if (vfx != null) vfx.SetActive(true);
    }
    void active_true()
    {
        gameObject.SetActive(true);
    }
}
