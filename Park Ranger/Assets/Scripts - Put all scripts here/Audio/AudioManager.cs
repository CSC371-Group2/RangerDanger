using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public float minTimeBetweenAudio;
    public float maxTimeBetweenAudio;
    public Sound[] sounds;

    public static AudioManager instance;

    private Sound[] autoPlays;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in  sounds)
        {
            s.source = s.origin.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 1;
        }
        autoPlays = Array.FindAll(sounds, sound => sound.autoPlay == true);
    }

    private void Start()
    {
        Play("BGM");
        StartCoroutine(autoPlaySound());
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Cannot find sound " + name);
            return;
        }
        s.source.Play();
    }

    IEnumerator autoPlaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenAudio, maxTimeBetweenAudio));
            string name = autoPlays[UnityEngine.Random.Range(0, autoPlays.Length)].name;
            Debug.Log(name);
            Play(name);
        }
    }
}
