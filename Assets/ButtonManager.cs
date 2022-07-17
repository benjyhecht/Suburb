using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Toggle toggle;
    float minPuzzleSize = 5;
    float maxPuzzleSize = 10;
    float puzzleSize = 5;
    float minPieceSize = 1;
    float maxPieceSize = 3;
    float pieceSize = 1;

    float minSongVolume = 0f;
    float maxSongVolume = .5f;
    float currentSongVolume = .5f;

    float minSFXVolume = 0f;
    float maxSFXVolume = .5f;
    float currentSFXVolume = .5f;

    DontDestroy dd;
    SongManager songManager;
    SFXManager sfxManager;

    private void Awake()
    {
        dd = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
        songManager = dd.GetComponent<SongManager>();
        sfxManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SFXManager>();
    }

    private void OnEnable()
    {
        if (slider != null)
        {
            if (slider.name.Contains("Puzzle"))
            {
                slider.value = (dd.GetPuzzleSize() - minPuzzleSize) / (maxPuzzleSize - minPuzzleSize);
            }
            else if (slider.name.Contains("Piece"))
            {
                slider.value = (dd.GetPieceSize() - minPieceSize) / (maxPieceSize - minPieceSize);
            }
            else if (slider.name.Contains("Music"))
            {
                slider.value = (songManager.GetVolume() - minSongVolume) / (maxSongVolume - minSongVolume);
            }
            else if (slider.name.Contains("SFX"))
            {
                slider.value = (sfxManager.GetVolume() - minSFXVolume) / (maxSFXVolume - minSFXVolume);
            }
        }
        if (toggle != null)
        {
            toggle.isOn = dd.GetRotationAllowed();
        }
    }

    public void PuzzleSizeChange()
    {
        float clampedValue = Mathf.Clamp(slider.value, 0, 1);
        puzzleSize = minPuzzleSize + (maxPuzzleSize - minPuzzleSize) * clampedValue;
        dd.SetPuzzleSize(puzzleSize);
    }

    public void PieceSizeChange()
    {
        float clampedValue = Mathf.Clamp(slider.value, 0, 1);
        pieceSize = minPieceSize + (maxPieceSize - minPieceSize) * clampedValue;
        dd.SetPieceSize(pieceSize);
    }

    public void RotationAllowedChange()
    {
        dd.SetRotationAllowed(toggle.isOn);
    }

    public void SongVolumeChange()
    {
        float clampedValue = Mathf.Clamp(slider.value, 0, 1);
        currentSongVolume = minSongVolume + (maxSongVolume - minSongVolume) * clampedValue;
        songManager.SetVolume(currentSongVolume);
    }

    public void SFXVolumeChange()
    {
        float clampedValue = Mathf.Clamp(slider.value, 0, 1);
        currentSFXVolume = minSFXVolume + (maxSFXVolume - minSFXVolume) * clampedValue;
        sfxManager.SetVolume(currentSFXVolume);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
