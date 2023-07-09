using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;


    private void Awake()
    {
        ChangeVol(PlayerPrefs.GetFloat(AudioPanel.musicKey, AudioPanel.defaultVol));
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        AudioPanel.ChangeMusicVol += ChangeVol;
    }

    private void OnDisable()
    {
        AudioPanel.ChangeMusicVol -= ChangeVol;
    }

    private void ChangeVol(float vol)
    {
        source.volume = vol * 0.25f;
    }
}
