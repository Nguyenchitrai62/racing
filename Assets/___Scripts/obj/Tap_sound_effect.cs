using UnityEngine;

public class Tap_sound_effect : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 0) gameObject.SetActive(false);
    }
}
