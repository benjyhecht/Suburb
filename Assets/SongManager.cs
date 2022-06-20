using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioClip[] songs;

    AudioSource audioSource;
    DontDestroy dd;

    AudioClip currentClip;
    float minVolume = 0f;
    float maxVolume = .5f;
    float currentVolume = .25f;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dd = GetComponent<DontDestroy>();
        currentVolume = dd.GetSongVolume();
        audioSource.clip = dd.GetCurrentClip();
        audioSource.volume = currentVolume;
    }

    private void Update()
    {
        if (!dd.GetPlaying() || Input.GetKeyUp(KeyCode.Space))
        {
            StartRandomSong();
        }
        if (!audioSource.isPlaying)
        {
            dd.SetPlaying(false);
        }
    }

    void StartRandomSong()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        dd.SetPlaying(true);
        currentClip = songs[Random.Range(0, songs.Length)];
        audioSource.clip = currentClip;
        dd.SetCurrentClip(currentClip);
        audioSource.Play();
    }

    public float GetVolume()
    {
        return currentVolume;
    }

    public void SetVolume(float volume)
    {
        currentVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        audioSource.volume = currentVolume;
        dd.SetSongVolume(currentVolume);
    }
}
