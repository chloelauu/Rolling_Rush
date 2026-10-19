using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sfx { Click, Crash, Fall, Stamp, Complete }

    public static AudioManager I;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip music, click, crash, fall, stamp, complete;
    public float musicVolume = 0.5f;

    void Awake()
    {
        // keep only one AudioManager, and keep it between scenes so music doesn't restart
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    System.Collections.IEnumerator Start()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = 0f;
        musicSource.Play();

        // fade in over 1.5 seconds, only when the game first starts
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / 1.5f;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t);
            yield return null;
        }
    }

    public static void Play(Sfx s)
    {
        if (I == null) return;
        AudioClip c = s switch
        {
            Sfx.Click => I.click,
            Sfx.Crash => I.crash,
            Sfx.Fall => I.fall,
            Sfx.Stamp => I.stamp,
            _ => I.complete,
        };
        if (c != null) I.sfxSource.PlayOneShot(c);
    }

    public static void SetPaused(bool paused)
    {
        if (I == null) return;
        I.musicSource.volume = paused ? I.musicVolume * 0.3f : I.musicVolume;
    }
}