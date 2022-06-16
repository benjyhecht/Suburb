using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    float minPuzzleSize = 5;
    float maxPuzzleSize = 10;
    float puzzleSize = 5;
    float minPieceSize = 1;
    float maxPieceSize = 3;
    float pieceSize = 1;

    DontDestroy dd;

    private void Awake()
    {
        dd = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
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
                print("Piece size: " + dd.GetPieceSize());
                print(slider.value);
            }
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

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
