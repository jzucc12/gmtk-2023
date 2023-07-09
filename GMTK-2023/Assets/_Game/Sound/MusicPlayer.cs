using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip mainSong;
    [SerializeField] private AudioClip bossSong;


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

    public void PlayMainSong()
    {
        if(source.clip == mainSong) return;
        source.Stop();
        source.clip = mainSong;
        source.Play();
    }

    public void PlayBossSong()
    {
        source.Stop();
        source.clip = bossSong;
        source.Play();
    }
}
