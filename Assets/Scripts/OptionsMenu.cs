using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
  [SerializeField] private AudioMixer sfxMixer;
  [SerializeField] private AudioMixer musicMixer;
  
  [SerializeField] private TextMeshProUGUI sfxTxt;
  [SerializeField] private TextMeshProUGUI musicTxt;

  public void SetSfxVolume(Single volume)
  {
    float vol = math.log10(volume) * 20f;
    sfxMixer.SetFloat("sfxVolume",vol);
    PlayerPrefs.SetFloat("sfxVolume", vol);
  }

  public void SetMusicVolume(Single volume)
  {
    float vol = math.log10(volume) * 20f;
    musicMixer.SetFloat("musicVolume",vol);
    PlayerPrefs.SetFloat("musicVolume", vol);
  }


  private void Awake()
  {
    if (PlayerPrefs.HasKey("musicVolume"))
    {
      SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
    }
    else
    {
      SetMusicVolume(0.4f);
    }
    if (PlayerPrefs.HasKey("sfxVolume"))
    {
      SetSfxVolume(PlayerPrefs.GetFloat("sfxVolume"));
    }
    else
    {
      SetSfxVolume(0.4f);
    }
  }
}