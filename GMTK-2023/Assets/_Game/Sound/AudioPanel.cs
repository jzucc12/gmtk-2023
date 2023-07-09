using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public static event Action<float> ChangeMusicVol;
    public static string musicKey => "music";
    public static string sfxKey => "sfx";
    public static float defaultVol => 0.5f;

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat(musicKey, defaultVol);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxKey, defaultVol);
    }

    private void OnEnable()
    {
        musicSlider.onValueChanged.AddListener(ChangeMusic);
        sfxSlider.onValueChanged.AddListener(ChangeSFX);
    }

    private void OnDisable()
    {
        musicSlider.onValueChanged.RemoveListener(ChangeMusic);
        sfxSlider.onValueChanged.RemoveListener(ChangeSFX);
    }

    private void ChangeMusic(float vol)
    {
        PlayerPrefs.SetFloat(musicKey, vol);
        ChangeMusicVol?.Invoke(vol);
    }

    private void ChangeSFX(float vol)
    {
        PlayerPrefs.SetFloat(sfxKey, vol);
    }
}
