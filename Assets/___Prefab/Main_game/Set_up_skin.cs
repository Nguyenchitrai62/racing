using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Set_up_skin : MonoBehaviour
{
    private Animator anim;

    public List<GameObject> skin;
    private int skin_value;

    void Awake()
    {
        anim = GetComponent<Animator>();

        foreach (GameObject obj in skin)
        {
            obj.SetActive(false);
        }
        skin_value = PlayerPrefs.GetInt("skin_value", 0);
        if (transform.name.StartsWith("BOT")) skin_value = Random.Range(1, skin.Count);
        anim.avatar = skin[skin_value].GetComponent<Animator>().avatar;
        skin[skin_value].SetActive(true);

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "kart") anim.SetInteger("state", 6);
        if (currentScene.name == "plappy_bird") anim.SetInteger("state", 5);
    }
}
