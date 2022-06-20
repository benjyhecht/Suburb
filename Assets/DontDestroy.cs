using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [Range(5, 10)] [SerializeField] float puzzleSize = 5;
    [Range(1, 3)][SerializeField] float pieceSize = 1;

    bool playing = false;
    AudioClip currentAudioClip;

    float songVolume = .25f;
    float sfxVolume = .25f;

    private void Awake()
    {
        GameObject[] allDDs = GameObject.FindGameObjectsWithTag("DontDestroy");
        print("DD count: " + allDDs.Length);
        if (allDDs.Length >1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public float GetPuzzleSize()
    {
        return puzzleSize;
    }

    public void SetPuzzleSize(float puzzleSize)
    {
        this.puzzleSize = puzzleSize;
    }

    public float GetPieceSize()
    {
        return pieceSize;
    }

    public void SetPieceSize(float pieceSize)
    {
        this.pieceSize = pieceSize;
    }

    public bool GetPlaying()
    {
        return playing;
    }

    public void SetPlaying(bool playing)
    {
        this.playing = playing;
    }

    public AudioClip GetCurrentClip()
    {
        return currentAudioClip;
    }

    public void SetCurrentClip(AudioClip currentAudioClip)
    {
        this.currentAudioClip = currentAudioClip;
    }

    public float GetSongVolume()
    {
        return songVolume;
    }

    public void SetSongVolume(float volume)
    {
        songVolume = volume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
