using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    //public AudioMixer soundEffectMixer;
    //public AudioMixer backgroundMusicMixer;

    private static readonly string FIRST_PLAY = "FirstPlay";
    private static readonly string BCK_MUSIC = "BackgroundPref";
    private static readonly string SOUND_FX = "SoundEffectsPref";

    public TextMeshProUGUI  musicVolumeText;
    public TextMeshProUGUI  soundEffectText;


    //public Slider backgroundMusicSlider, soundEfectSlider;
    private float backgroundMusicVolume, soundEffectVolume;
    private int firstPlayInt;


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(string.Format("FirstPlay is  {0}", PlayerPrefs.GetInt(FIRST_PLAY)));
        firstPlayInt = PlayerPrefs.GetInt(FIRST_PLAY);
        if (firstPlayInt == 0) // default value
        {

            backgroundMusicVolume = 1f;
            soundEffectVolume = 1f;  
            PlayerPrefs.SetFloat(BCK_MUSIC, backgroundMusicVolume);
            PlayerPrefs.SetFloat(SOUND_FX, soundEffectVolume);    
             
        }
        else
        {
            backgroundMusicVolume = PlayerPrefs.GetFloat(BCK_MUSIC);
            soundEffectVolume = PlayerPrefs.GetFloat(SOUND_FX);
            UpdateTextValues();

            Debug.Log(string.Format("Save PlayerPrefs {0} and {1}", backgroundMusicVolume, soundEffectVolume));

        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetInt(FIRST_PLAY, 1);
        PlayerPrefs.SetFloat(BCK_MUSIC, backgroundMusicVolume);
        PlayerPrefs.SetFloat(SOUND_FX, soundEffectVolume);
    }


    private void OnApplicationFocus(bool inFocus)
    {
        Debug.Log("OnApplicationFocus");
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateMusicVolume(float newVolume)
    {
        backgroundMusicVolume = newVolume;
        masterMixer.SetFloat("musicVolumeParam", backgroundMusicVolume);

        UpdateTextValues();
        SaveSoundSettings();
    }

    public void UpdateSoundEffect(float newVolume)
    {
        soundEffectVolume = newVolume;

        masterMixer.SetFloat("soundEffectParam", backgroundMusicVolume);
        UpdateTextValues();
        SaveSoundSettings();
    }


    public void UpdateTextValues()
    {
        Debug.Log(string.Format("UpdateText Values  :: {0} and {1} ", backgroundMusicVolume, soundEffectVolume));
        //musicVolumeText.text = ((int)(backgroundMusicVolume * 100f)).ToString();
        //soundEffectText.text = ((int)(soundEffectVolume * 100f)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
