using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClip[] pickUpPieces;
    [SerializeField] AudioClip[] putDownPieces;
    [SerializeField] AudioClip[] victoryFanfares;

    AudioSource audioSource;
    float minVolume = 0f;
    float maxVolume = .5f;
    float currentVolume = .25f;

    DontDestroy dd;

    private void Awake()
    {
        dd = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
        currentVolume = dd.GetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
        {
            try
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.volume = currentVolume;
            }
            catch (Exception e)
            {
                print(e);
            }
        }
    }

    public void PlaySound(Enums.SFX soundIn)
    {
        AudioClip[] usedClips = null;
        switch (soundIn)
        {
            case Enums.SFX.PICKUP:
                usedClips = pickUpPieces;
                break;
            case Enums.SFX.PUTDOWN:
                usedClips = putDownPieces;
                break;
            case Enums.SFX.WIN:
                usedClips = victoryFanfares;
                break;
            default:
                break;
        }
        if (usedClips != null)
        {
            foreach (AudioClip audioClip in usedClips)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    public float GetVolume()
    {
        return currentVolume;
    }

    public void SetVolume(float volume)
    {
        currentVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        audioSource.volume = currentVolume;
        dd.SetSFXVolume(currentVolume);
    }
}
