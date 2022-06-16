using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [Range(5, 10)] [SerializeField] float puzzleSize = 5;
    [Range(1, 3)][SerializeField] float pieceSize = 1;

    private void Awake()
    {
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
}
