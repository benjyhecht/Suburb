using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    [Range(5, 10)] [SerializeField] int puzzleSize = 10;
    [Range(1, 3)] [SerializeField] int pieceSize = 2;
    [SerializeField] GameObject basePlate;

    Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };
    bool[][] positions;
    List<List<Vector2>> allPieces = new List<List<Vector2>>();
    List<Vector2> allLocations = new List<Vector2>();
    List<Vector2> openLocations = new List<Vector2>();
    BlockPlacer blockPlacer;

    private void Awake()
    {
        DontDestroy dd = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
        puzzleSize = Mathf.Clamp(Mathf.RoundToInt(dd.GetPuzzleSize()), 5, 10);
        pieceSize = Mathf.Clamp(Mathf.RoundToInt(dd.GetPieceSize()), 1, 3);
    }

    void Start()
    {
        positions = new bool[puzzleSize][];
        blockPlacer = GetComponent<BlockPlacer>();
        for (int v = 0; v < puzzleSize; v++)
        {
            positions[v] = new bool[puzzleSize];
            for (int h = 0; h < puzzleSize; h++)
            {
                positions[v][h] = false;
                allLocations.Add(new Vector2(h, v));
                openLocations.Add(new Vector2(h, v));
                Instantiate(basePlate, new Vector3(h, -.01f, v), Quaternion.identity, this.transform);
            }
        }

        while (openLocations.Count > 0)
        {
            List<Vector2> newPieceList = new List<Vector2>();
            CreatePiece(newPieceList);
            allPieces.Add(newPieceList);
        }

        blockPlacer.InstantiatePiece(allPieces, puzzleSize);

        Vector3 lookLocation = new Vector3(puzzleSize / 2f - .5f, 0, puzzleSize / 2f - .5f);
        Camera.main.GetComponent<CameraController>().ChangeLookLocation(lookLocation, puzzleSize * 2);
    }

    void CreatePiece(List<Vector2> pieceList)
    {
        List<Vector2> possibleNeighbourStartingLocations = null;
        possibleNeighbourStartingLocations = new List<Vector2>();
        Vector2 startLocation = openLocations[UnityEngine.Random.Range(0, openLocations.Count)];
        openLocations.Remove(startLocation);
        positions[(int)startLocation.x][(int)startLocation.y] = true;
        pieceList.Add(startLocation);
        possibleNeighbourStartingLocations.Add(startLocation);

        Vector2 nextLocation = GetNeighbour(startLocation);
        bool currentlyAddingBlocks = true;
        float countDown = puzzleSize + (pieceSize - 1) / 2f * puzzleSize;
        while (currentlyAddingBlocks)
        {
            if (possibleNeighbourStartingLocations.Count == 0)
            {
                print("out of locations");
                currentlyAddingBlocks = false;
                break;
            }
            else if (UnityEngine.Random.Range(0, 100) > 75 + puzzleSize + 5 * pieceSize)
            {
                print("Rolled to end");
                currentlyAddingBlocks = false;
                break;
            }
            else if (countDown <= 0)
            {
                print("Too many pieces");
                currentlyAddingBlocks = false;
                break;
            }
            else
            {
                startLocation = possibleNeighbourStartingLocations[UnityEngine.Random.Range(0, possibleNeighbourStartingLocations.Count)];
                nextLocation = GetNeighbour(startLocation);
                if (startLocation == nextLocation)
                {
                    possibleNeighbourStartingLocations.Remove(nextLocation);
                }
                else
                {
                    openLocations.Remove(nextLocation);
                    possibleNeighbourStartingLocations.Add(nextLocation);
                    positions[(int)nextLocation.x][(int)nextLocation.y] = true;
                    pieceList.Add(nextLocation);
                    countDown--;
                }
            }
        }
    }

    Vector2 GetNeighbour(Vector2 blockPosition)
    {
        List<Vector2> cardinalDirections = new List<Vector2>();
        cardinalDirections.Add(new Vector2(1, 0));
        cardinalDirections.Add(new Vector2(0, -1));
        cardinalDirections.Add(new Vector2(-1, 0));
        cardinalDirections.Add(new Vector2(0, -1));

        Vector2 returnedVector = new Vector2(0, 0);
        List<Vector2> possibleDirections = new List<Vector2>();
        if (blockPosition.x <= 0f)
        {
            cardinalDirections.Remove(new Vector2(-1, 0));
        }
        if (blockPosition.x >= puzzleSize - 1f)
        {
            cardinalDirections.Remove(new Vector2(1, 0));
        }
        if (blockPosition.y <= 0f)
        {
            cardinalDirections.Remove(new Vector2(0, -1));
        }
        if (blockPosition.y >= puzzleSize - 1f)
        {
            cardinalDirections.Remove(new Vector2(0, 1));
        }

        for (int index = 0; index < cardinalDirections.Count; index++)
        {
            Vector2 tempPosition = blockPosition + cardinalDirections[index];
            try
            {
                if (positions[(int)tempPosition.x][(int)tempPosition.y] == false)
                {
                    possibleDirections.Add(cardinalDirections[index]);
                }
            }
            catch (Exception e)
            {
                //print(e);
            }
        }
        if (possibleDirections.Count > 0)
        {
            returnedVector = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
        }

        return returnedVector + blockPosition;
    }
}
