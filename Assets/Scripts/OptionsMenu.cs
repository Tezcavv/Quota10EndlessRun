using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
  [SerializeField] private AudioMixer sfxMixer;
  [SerializeField] private AudioMixer musicMixer;

  [SerializeField] private Slider sfxSlider;
  [SerializeField] private Slider musicSlider;

  [SerializeField] private TextMeshProUGUI sfxText;
  [SerializeField] private TextMeshProUGUI musicText;
  
  private const string SFX_KEY = "sfxVolume";
  private const string MUSIC_KEY = "musicVolume";

  public void SetSfxVolume(float value)
  {
    float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    sfxMixer.SetFloat("sfxVolume", dB);
    PlayerPrefs.SetFloat(SFX_KEY, value); 
    UpdateVolumeText(sfxText,value);
  }

  public void SetMusicVolume(float value)
  {
    float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    musicMixer.SetFloat("musicVolume", dB);
    PlayerPrefs.SetFloat(MUSIC_KEY, value); 
    UpdateVolumeText(musicText,value);
  }

  private void Awake()
  {
    
    gameObject.SetActive(false);
    
    float musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, 0.4f);
    float sfxValue   = PlayerPrefs.GetFloat(SFX_KEY, 0.4f);

    musicSlider.SetValueWithoutNotify(musicValue);
    sfxSlider.SetValueWithoutNotify(sfxValue);

    SetMusicVolume(musicValue);
    SetSfxVolume(sfxValue);
  }
  
  private void UpdateVolumeText(TextMeshProUGUI txt, float value)
  {
    if (value <= 0.001f)
      txt.text = "MUTED";
    else
      txt.text = Mathf.RoundToInt(value * 100f) + "%";
  }
}