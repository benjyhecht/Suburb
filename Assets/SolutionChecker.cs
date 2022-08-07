using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionChecker : MonoBehaviour
{
    Vector2[] openSpaces;
    bool paused = false;
    bool gameOver = false;
    List<GameObject> gamePieces;
    int puzzleSize;

    public void SetOpenSpaces(Vector2[] openSpaces)
    {
        this.openSpaces = openSpaces;
    }

    public bool GetPlayable()
    {
        return !(paused || gameOver);
    }

    public void SetPaused(bool paused)
    {
        this.paused = paused;
    }

    public bool GetPaused()
    {
        return paused;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void RegisterPieces(List<GameObject> gamePieces, int puzzleSize)
    {
        this.gamePieces = gamePieces;
        this.puzzleSize = puzzleSize;
    }

    public void CheckGameOver()
    {
        bool gameOver = true;
        foreach(GameObject piece in gamePieces)
        {
            PieceProperties pp = piece.GetComponent<PieceProperties>();
            pp.CalculateExtents(false);
            //Vector2 minExtents = new Vector2 (pp.transform.position.x, pp.transform.position.z) - pp.GetMinExtents();
            //Vector2 maxExtents = pp.GetMaxExtents() + new Vector2(pp.transform.position.x, pp.transform.position.z);
            Vector2 minExtents = pp.GetMinExtents();
            Vector2 maxExtents = pp.GetMaxExtents();
            string message = pp.name;
            message += " Min Extents: " + minExtents + " || Max Extents: " + maxExtents;
            if (minExtents.x < -.1f)
            {
                gameOver = false;
                message += " (min x too low: " + minExtents.x + ") ";
            }
            if (minExtents.y < -.1f)
            {
                gameOver = false;
                message += " (min y too low: " + minExtents.y + ") ";
            }
            if (maxExtents.x > puzzleSize - .9f)
            {
                gameOver = false;
                message += " (max x too high: " + maxExtents.x + ") ";
            }
            if (maxExtents.y > puzzleSize - .9f)
            {
                gameOver = false;
                message += " (max y too high: " + maxExtents.y + ") ";
            }
            print(message);
        }
        print("");

        if (gameOver)
        {
            this.gameOver = true;
            GameObject button = GameObject.FindGameObjectWithTag("MainCanvas").transform.GetChild(1).gameObject;
            button.GetComponent<MenuShower>().ButtonShowMenu();
            GetComponent<SFXManager>().PlaySound(Enums.SFX.WIN);
            Camera.main.GetComponent<CameraController>().EndGame();
            Timer timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            timer.endTime();
        }
    }

    public int GetPuzzleSize()
    {
        return puzzleSize;
    }
}
