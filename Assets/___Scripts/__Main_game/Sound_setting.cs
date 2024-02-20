using UnityEngine;

public class Sound_setting : MonoBehaviour
{
    public GameObject sound_button;
    public GameObject music_button;
    public GameObject Tap;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 0) sound_button.SetActive(false);
        if (PlayerPrefs.GetInt("music", 1) == 0) music_button.SetActive(false);
    }
    public void sound_on()
    {
        Tap.SetActive(true);
        PlayerPrefs.SetInt("sound", 1);
    }
    public void sound_off()
    {
        Tap.SetActive(false);
        PlayerPrefs.SetInt("sound", 0);
    }
    public void music_on()
    {
        PlayerPrefs.SetInt("music", 1);
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);
    }
    public void music_off()
    {
        PlayerPrefs.SetInt("music", 0);
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
    }
}
