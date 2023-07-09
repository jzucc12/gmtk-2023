using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<SFXData> sounds;


    public void PlaySound(string sound)
    {
        AudioClip clip = sounds.Where((data) => data.name == sound.ToLower()).First().sfx;
        PlaySound(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        source.Stop();
        source.volume = PlayerPrefs.GetFloat(AudioPanel.sfxKey, AudioPanel.defaultVol) * 0.5f;
        source.clip = clip;
        source.Play();
    }
}

[System.Serializable]
public struct SFXData
{
    public string name;
    public AudioClip sfx;
}