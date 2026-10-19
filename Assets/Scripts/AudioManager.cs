using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum Sfx { Click, Crash, Fall, Stamp, Complete }

    public static AudioManager I;   // the one AudioManager in charge (singleton)

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip music, click, crash, fall, stamp, complete;

    [Header("Music volume")]
    public float menuVolume = 0.5f;    // menu and credits
    public float levelVolume = 0.25f;  // while playing a level
    public float pausedFactor = 0.3f;  // how much quieter when paused / on result cards
    public float fadeSpeed = 0.5f;     // how fast volume changes (per second)

    [Header("SFX volume")]
    [Range(0f, 1f)] public float clickVolume = 0.6f;
    [Range(0f, 1f)] public float crashVolume = 1f;
    [Range(0f, 1f)] public float fallVolume = 1f;
    [Range(0f, 1f)] public float stampVolume = 1f;
    [Range(0f, 1f)] public float completeVolume = 0.8f;

    float target;   // the volume we're gliding towards
    bool paused;
    bool inLevel;

    void Awake()
    {
        // keep only one AudioManager, and keep it between scenes so music doesn't restart
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (I == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // a scene with a GameManager is a level
        inLevel = FindObjectOfType<GameManager>() != null;
        paused = false;
        UpdateTarget();
    }

    void Start()
    {
        if (I != this) return;   // extra copies do nothing

        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = 0f;   // starts silent, then glides up = fade-in
        musicSource.Play();
        UpdateTarget();
    }

    void Update()
    {
        if (I != this) return;
        // smoothly move towards the target volume (works even when the game is paused)
        musicSource.volume = Mathf.MoveTowards(musicSource.volume, target, fadeSpeed * Time.unscaledDeltaTime);
    }

    void UpdateTarget()
    {
        float baseVol = inLevel ? levelVolume : menuVolume;
        target = paused ? baseVol * pausedFactor : baseVol;
    }

    public static void Play(Sfx s)
    {
        if (I == null) return;
        (AudioClip c, float v) = s switch
        {
            Sfx.Click => (I.click, I.clickVolume),
            Sfx.Crash => (I.crash, I.crashVolume),
            Sfx.Fall => (I.fall, I.fallVolume),
            Sfx.Stamp => (I.stamp, I.stampVolume),
            _ => (I.complete, I.completeVolume),
        };
        if (c != null) I.sfxSource.PlayOneShot(c, v);
    }

    public static void SetPaused(bool value)
    {
        if (I == null) return;
        I.paused = value;
        I.UpdateTarget();
    }
}