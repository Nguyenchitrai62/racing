using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Transitions : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Animate());
    }
    private IEnumerator Animate()
    {
        while (true)
        {
            
            yield return null;
        }
    }
    public void close_effect()
    {
        anim.SetInteger("state", 1);
    }
    public void open_effect()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => anim.SetInteger("state", 0));
    }
    public void load_cur_scene()
    {
        anim.SetInteger("state", 1);
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() => SceneManager.LoadSceneAsync("game_1"));
    }
}
