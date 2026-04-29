using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    public static AudioManager Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayRandomSFX(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        sfxAudioSource.PlayOneShot(clips[randomIndex]);
    }

    public void PlayMusic(AudioClip newMusic)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = newMusic;
        musicAudioSource.Play();
    }
}
