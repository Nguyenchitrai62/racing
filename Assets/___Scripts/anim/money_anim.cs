using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class money_anim : MonoBehaviour
{
    //anim tiền bay 1,2s sau khi kết thúc enable các button


    public RectTransform objectToPool;
    public RectTransform targetTransform;
    private List<RectTransform> objectPool;
    public List<Button> enable_button;

    private void Awake()
    {
        objectPool = new List<RectTransform>();
        for (int i = 0; i < 16; i++)
        {
            RectTransform obj = Instantiate(objectToPool);
            obj.transform.SetParent(transform.parent, false);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }
    }

    void OnEnable()
    {
        //AnimateObjects();
    }

    public void AnimateObjects()
    {
        for (int i = 0; i < 8; i++)
        {
            RectTransform obj = Get_Object_From_Pool();
            obj.gameObject.SetActive(true);
            obj.anchoredPosition = obj.transform.InverseTransformPoint(Input.mousePosition);
            Vector3 temp = obj.anchoredPosition + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));

            obj.DOAnchorPos(temp, 0.5f).SetUpdate(true); // Sử dụng unscaled time
            obj.DOAnchorPos(targetTransform.anchoredPosition, 0.7f).SetDelay(0.5f).SetUpdate(true)
            .OnComplete(() =>
            {
                obj.gameObject.SetActive(false);
            });
        }

        DOVirtual.DelayedCall(1.2f, set_active_false, true); // true ở cuối để sử dụng unscaled time
    }
    void set_active_false()
    {
        for (int i = 0; i < enable_button.Count; i++)
        {
            enable_button[i].enabled = true;
        }
    }
    public RectTransform Get_Object_From_Pool()
    {
        foreach (RectTransform obj in objectPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        RectTransform newObj = Instantiate(objectToPool);
        newObj.transform.SetParent(transform.parent, false);
        newObj.gameObject.SetActive(false);
        objectPool.Add(newObj);
        return newObj;
    }

}
