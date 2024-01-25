using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private AudioSource audioSource;
    public static Sound_Manager Instance;

    public List<AudioClip> SFX;
    public List<GameObject> sound_effect;

    int pev_timescale = 1;
    private void Reset()
    {
#if UNITY_EDITOR
        Load_Component();
#endif
    }
    void Load_Component()
    {
        SFX = new List<AudioClip>();
        sound_effect = new List<GameObject>();
        foreach (Transform obj in transform)
        {
            SFX.Add(obj.GetComponent<AudioSource>().clip);
            sound_effect.Add(obj.gameObject);
        }
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Time.timeScale == 0 && pev_timescale == 1)
        {
            for (int i = 0; i < sound_effect.Count; i++)
            {
                sound_effect[i].GetComponent<AudioSource>().Pause();
            }
            pev_timescale = 0;
        }
        if (Time.timeScale == 1 && pev_timescale == 0)
        {
            for (int i = 0; i < sound_effect.Count; i++)
            {
                sound_effect[i].GetComponent<AudioSource>().UnPause();
            }
            pev_timescale = 1;
        }
    }

    public void Play_Sound(string name)
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            int index = 0;
            for (int i = 0; i < SFX.Count; i++) if (SFX[i].name == name) index = i;
            audioSource.PlayOneShot(SFX[index]);
        }
    }

    public void Play_Music(string name, int i)
    {
        if (audioSource.clip != null)
        {
            if (audioSource.clip.name != name || (i == 0 && audioSource.isPlaying) || (i == 1 && !audioSource.isPlaying))
            {
                foreach (AudioClip clip in SFX)
                {
                    if (clip.name == name)
                    {
                        audioSource.clip = clip;
                        if (i == 1 && PlayerPrefs.GetInt("music", 1) == 1) audioSource.Play();
                        if (i == 0) audioSource.Stop();
                        return;
                    }
                }
            }
        }
        else
        {
            foreach (AudioClip clip in SFX)
            {
                if (clip.name == name)
                {
                    audioSource.clip = clip;
                    if (i == 1 && PlayerPrefs.GetInt("music", 1) == 1) audioSource.Play();
                    if (i == 0) audioSource.Stop();
                    return;
                }
            }
        }
    }




    public void Play_sound_effect(int i)
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)

            sound_effect[i].GetComponent<AudioSource>().enabled = true;
    }
    public void Stop_sound_effect(int i)
    {
        sound_effect[i].GetComponent<AudioSource>().enabled = false;
    }




    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        audioSource.volume = volume;
    }





    public void stop_all_sound_effect()
    {
        for (int i = 0; i < sound_effect.Count; i++)
        {
            sound_effect[i].GetComponent<AudioSource>().enabled = false;
        }
    }
}
