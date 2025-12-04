using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsAudioUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioReference audioRef;

    // Start is called before the first frame update
    void Start()
    {
        audioRef = GetComponent<AudioReference>();
        if (audioRef == null || audioRef.audioManager == null) return;

        var am = audioRef.audioManager;

        if (masterSlider != null)
        {
            masterSlider.value = am.GetMasterVolume();
            masterSlider.onValueChanged.AddListener(am.SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = am.GetMusicVolume();
            musicSlider.onValueChanged.AddListener(am.SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = am.GetSfxVolume();
            sfxSlider.onValueChanged.AddListener(am.SetSfxVolume);
        }
    }

}
