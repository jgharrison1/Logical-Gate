using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider FXSlider;
    public Slider musicSlider;

    void Start() {
        if (PlayerPrefs.GetFloat("masterLevel")!= 0){
            SetMasterVolume(PlayerPrefs.GetFloat("masterLevel"));
        }
        if (PlayerPrefs.GetFloat("FXLevel")!= 0) {
            SoundFXVolume(PlayerPrefs.GetFloat("fxLevel"));
        }
        if (PlayerPrefs.GetFloat("musicLevel")!= 0) {
            SoundMusicVolume(PlayerPrefs.GetFloat("musicLevel"));
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f); //This changes how volume scales from logarithmic scaling to linear
        PlayerPrefs.SetFloat("masterLevel", level);
        masterSlider.value = level;
    }

    public void SoundFXVolume(float level) 
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("FXLevel", level);
        FXSlider.value = level;
    }

    public void SoundMusicVolume(float level) 
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("musicLevel", level);
        musicSlider.value = level;
    }
}
