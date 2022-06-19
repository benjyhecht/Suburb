using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClip pickUpPiece;
    [SerializeField] AudioClip putDownPiece;
    [SerializeField] AudioClip victoryFanfare;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
        {
            try
            {
                audioSource = GetComponent<AudioSource>();
            }
            catch (Exception e)
            {
                print(e);
            }
        }
    }

    public void PlaySound(Enums.SFX soundIn)
    {
        switch (soundIn)
        {
            case Enums.SFX.PICKUP:
                audioSource.PlayOneShot(pickUpPiece);
                break;
            case Enums.SFX.PUTDOWN:
                audioSource.PlayOneShot(putDownPiece);
                break;
            case Enums.SFX.WIN:
                audioSource.PlayOneShot(victoryFanfare);
                break;
            default:
                break;
        }
    }


}
