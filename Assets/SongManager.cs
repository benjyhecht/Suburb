using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioClip[] songs;

    AudioSource audioSource;
    DontDestroy dd;

    AudioClip currentClip;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dd = GetComponent<DontDestroy>();
        audioSource.clip = dd.GetCurrentClip();
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
}
