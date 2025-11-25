using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    //[Header("SFX Clips")]
    [Serializable]

    public class Sound
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> sfxClips = new List<Sound>();

    private Dictionary<string, Sound> sfxDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildSfxDictionary();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void BuildSfxDictionary()
    {
        sfxDict = new Dictionary<string, Sound>();
        foreach (var s in sfxClips)
        {
            if (s != null && s.clip != null && !string.IsNullOrEmpty(s.id))
            {
                sfxDict[s.id] = s;
            }
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleMusicForScene(scene.name);
    }

    private void HandleMusicForScene(string sceneName)
    {
        AudioClip targetClip = null;

        //Same music for MainMenu and OptionsMenu
        if (sceneName == "MainMenu" || sceneName == "OptionsScene")
        {
            targetClip = menuMusic;
        }

        //New music in LevelSelect and kept
        else if (sceneName == "LevelSelector" || sceneName == "LevelScene")
        {
            targetClip = levelMusic;
        }

        if (targetClip == null)
            return;

        //If the right clip is playing, don't restart
        if (musicSource.clip == targetClip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = targetClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    //Dictionary for sound effects
    public void PlaySFX(string id)
    {
        if (sfxDict == null)
        {
            BuildSfxDictionary();
        }

        if (sfxDict.TryGetValue(id, out var sound))
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning("No SFX with that id found");
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }
}
