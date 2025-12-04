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

    [Header("Volume (0-1")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private const string MASTER_KEY = "Volume_Master";
    private const string MUSIC_KEY = "Volume_Music";
    private const string SFX_KEY = "Volume_SFX";

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
        LoadVolumes();
        ApplyVolumes();

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
        if (sceneName == "MainMenu" || sceneName == "Options")
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

    //Volume
    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey(MASTER_KEY))
        {
            masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        }

        if (PlayerPrefs.HasKey(MUSIC_KEY))
        {
            musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        }

        if (PlayerPrefs.HasKey(SFX_KEY))
        {
            sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        }
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat(MASTER_KEY, masterVolume);
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = masterVolume * musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = masterVolume * sfxVolume;
        }
    }

    //UI sliders
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolumes();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolumes();
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;

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
