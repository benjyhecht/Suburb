using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    private enum placeLocation { LEFT, TOP, RIGHT, BOTTOM};

    [SerializeField] GameObject [] basicBlockFills;
    [SerializeField] GameObject [] allPrefabs;

    List<Vector2> cardinalDirections = new List<Vector2>();
    readonly Vector2 east = new Vector2(1, 0);
    readonly Vector2 south = new Vector2(0, -1);
    readonly Vector2 west = new Vector2(-1, 0);
    readonly Vector2 north = new Vector2(0, 1);

    List<GameObject> pieceParents = new List<GameObject>();
    List<GameObject> moveablePieces = new List<GameObject>();

    bool schoolTaken = false;
    bool streetTaken = false;
    bool yardTaken = false;

    int puzzleSize;

    GameObject ground;
    SolutionChecker sc;

    public void Awake()
    {
        cardinalDirections.Add(east);
        cardinalDirections.Add(south);
        cardinalDirections.Add(west);
        cardinalDirections.Add(north);

        ground = GameObject.FindGameObjectWithTag("Ground");
        sc = GameObject.FindGameObjectWithTag("GameController").GetComponent<SolutionChecker>();
    }

    public void InstantiatePiece(List<List<Vector2>> allPieces, int puzzleSize)
    {
        this.puzzleSize = puzzleSize;
        allPieces.Sort(SortBySizee);
        allPieces.Reverse();
        foreach (List<Vector2> pieceList in allPieces)
        {
            int envIndex = 1;
            if (schoolTaken)
            {
                envIndex = 2;

                if (streetTaken)
                {
                    envIndex = 0;
                }
                else
                {
                    streetTaken = true;
                }
            }
            else
            {
                schoolTaken = true;
            }

            Enums.Environment env = (Enums.Environment) envIndex;
            GameObject empty = new GameObject();
            empty.AddComponent<PieceProperties>();
            empty.transform.position = new Vector3(pieceList[0].x, 0, pieceList[0].y);
            empty.transform.rotation = Quaternion.identity;
            empty.transform.parent = null;
            empty.name = env.ToString();
            pieceParents.Add(empty);

            if (pieceList.Count == 1)
            {
                GameObject singleBlock = Instantiate(GetBlock(Enums.Environment.NA, Enums.BlockType.NA), new Vector3(pieceList[0].x, 0, pieceList[0].y), Quaternion.Euler(-90, 0, 0), null);
                //singleBlock.GetComponent<MeshRenderer>().material.color = Color.black;
                singleBlock.transform.localScale = new Vector3(.1f, .1f, .1f);
                singleBlock.transform.parent = this.transform;
                GameObject singleBlockFiller = Instantiate(basicBlockFills[Random.Range(0, basicBlockFills.Length)]);
                singleBlockFiller.transform.position = singleBlock.transform.position;
                singleBlockFiller.transform.rotation = Quaternion.Euler(0, 90 * Random.Range(0, 3), 0);
            }
            else
            {
                empty.AddComponent<PieceMover>();
                Color pieceColor = new Color(UnityEngine.Random.Range(0, 256) / 256f, UnityEngine.Random.Range(0, 256) / 256f, UnityEngine.Random.Range(0, 256) / 256f);

                GameObject blockToPlace = GetBlock(Enums.Environment.NA, Enums.BlockType.NA);
                int rotation = 0;

                yardTaken = false;

                foreach (Vector2 vector2 in pieceList)
                {
                    string blockName = "other";
                    List<Vector2> neighbours = CheckLocationOfNeighboors(vector2, pieceList);
                    if (neighbours.Count == 1)
                    {
                        if (env == Enums.Environment.HOUSE)
                        {
                            if (yardTaken)
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.DEADEND);
                                while (blockToPlace.name.Contains("Yard"))
                                {
                                    blockToPlace = GetBlock(env, Enums.BlockType.DEADEND);
                                }
                            }
                            else
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.DEADEND);
                                if (blockToPlace.name.Contains("Yard"))
                                {
                                    yardTaken = true;
                                }
                            }
                        }
                        else
                        {
                            blockToPlace = GetBlock(env, Enums.BlockType.DEADEND);
                        }

                        if (neighbours[0] == north)
                        {
                            rotation = 2;
                            blockName = "north";
                        }
                        else if (neighbours[0] == east)
                        {
                            rotation = 3;
                            blockName = "east";
                        }
                        else if (neighbours[0] == south)
                        {
                            rotation = 0;
                            blockName = "south";
                        }
                        else if (neighbours[0] == west)
                        {
                            rotation = 1;
                            blockName = "west";
                        }
                    }
                    else if (neighbours.Count == 2)
                    {
                        blockToPlace = GetBlock(Enums.Environment.NA, Enums.BlockType.NA);

                        if (neighbours.Contains(north) && neighbours.Contains(south))
                        {
                            blockToPlace = GetBlock(env, Enums.BlockType.STRAIGHT);
                            rotation = 0;
                            blockName = "north-South";
                        }
                        else if (neighbours.Contains(east) && neighbours.Contains(west))
                        {
                            blockToPlace = GetBlock(env, Enums.BlockType.STRAIGHT);
                            rotation = 1;
                            blockName = "east-west";
                        }
                        else if (neighbours.Contains(north))
                        {
                            if (neighbours.Contains(east))
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.CORNER);
                                rotation = 1;
                                blockName = "north-east";
                            }
                            else if (neighbours.Contains(west))
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.CORNER);
                                rotation = 0;
                                blockName = "north-west";
                            }
                        }
                        else if (neighbours.Contains(south))
                        {
                            if (neighbours.Contains(east))
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.CORNER);
                                rotation = 2;
                                blockName = "south-east";
                            }
                            else if (neighbours.Contains(west))
                            {
                                blockToPlace = GetBlock(env, Enums.BlockType.CORNER);
                                rotation = 3;
                                blockName = "south-west";
                            }
                        }
                    }
                    else if (neighbours.Count == 3)
                    {
                        blockToPlace = GetBlock(env, Enums.BlockType.JUNCTION);

                        if (!neighbours.Contains(north))
                        {
                            rotation = 0;
                            blockName = "n-north";
                        }
                        else if (!neighbours.Contains(east))
                        {
                            rotation = 1;
                            blockName = "n-east";
                        }
                        else if (!neighbours.Contains(south))
                        {
                            rotation = 2;
                            blockName = "n-south";
                        }
                        else if (!neighbours.Contains(west))
                        {
                            rotation = 3;
                            blockName = "n-west";
                        }
                    }
                    else
                    {
                        blockToPlace = GetBlock(env, Enums.BlockType.XROADS);
                        blockName = "x-road";
                    }
                    GameObject singleBlock = Instantiate(blockToPlace, new Vector3(vector2.x, 0, vector2.y), Quaternion.identity, null);
                    singleBlock.transform.parent = empty.transform;
                    singleBlock.transform.rotation = Quaternion.Euler(-90, 90 * rotation, 0);
                    singleBlock.transform.localScale = new Vector3(.1f, .1f, .1f);
                    if (env == Enums.Environment.HOUSE)
                    {
                        Material[] mats = singleBlock.GetComponent<MeshRenderer>().materials;
                        mats[singleBlock.GetComponent<PrefabProperties>().GetMatChange()].color = pieceColor;
                    }
                    singleBlock.GetComponent<PrefabProperties>().CalculateColors();
                    singleBlock.name = blockName + " " + (Enums.Environment)envIndex;
                }
                empty.GetComponent<PieceProperties>().CalculateExtents();
                moveablePieces.Add(empty);
            }
        }
        RemovePieces();
    }

    List<Vector2> CheckLocationOfNeighboors(Vector2 block, List<Vector2> pieceList)
    {
        List<Vector2> neighbourList = new List<Vector2>();

        foreach (Vector2 vector2 in cardinalDirections)
        {
            Vector2 checkedLocation = block + vector2;
            if (pieceList.Contains(checkedLocation))
            {
                neighbourList.Add(vector2);
            }
        }

        return neighbourList;
    }

    GameObject GetBlock(Enums.Environment environment, Enums.BlockType blockType)
    {
        List<GameObject> blocks = new List<GameObject>();
        foreach(GameObject prefab in allPrefabs)
        {
            PrefabProperties prefabProps = prefab.GetComponent<PrefabProperties>();
            if (prefabProps.GetEnvironment() == environment && prefabProps.GetBlockType() == blockType)
            {
                blocks.Add(prefab);
            }
        }

        return blocks[Random.Range(0, blocks.Count)];
    }

    static int SortBySizee(List<Vector2> list1, List<Vector2> list2)
    {
        return list1.Count.CompareTo(list2.Count);
    }

    public void RemovePieces()
    {
        for (int t = 0; t < moveablePieces.Count; t++)
        {
            GameObject tmp = moveablePieces[t];
            int r = Random.Range(t, moveablePieces.Count);
            moveablePieces[t] = moveablePieces[r];
            moveablePieces[r] = tmp;
        }

        List<GameObject> widerPieces = new List<GameObject>();
        List<GameObject> tallerPieces = new List<GameObject>();
        List<GameObject> squarePieces = new List<GameObject>();

        foreach (GameObject piece in moveablePieces)
        {
            Vector2 minExtents = piece.GetComponent<PieceProperties>().GetMinExtents();
            Vector2 maxExtents = piece.GetComponent<PieceProperties>().GetMaxExtents();
            Vector2 extents = maxExtents - minExtents;
            if (extents.x > extents.y)
            {
                widerPieces.Add(piece);
            }
            else if (extents.y > extents.x)
            {
                tallerPieces.Add(piece);
            }
            else
            {
                squarePieces.Add(piece);
            }
        }

        while (squarePieces.Count > 0)
        {
            if (widerPieces.Count < tallerPieces.Count)
            {
                widerPieces.Add(squarePieces[0]);
            }
            else
            {
                tallerPieces.Add(squarePieces[0]);
            }

            squarePieces.Remove(squarePieces[0]);
        }

        if (Mathf.Abs(tallerPieces.Count - widerPieces.Count) > 2)
        {
            if (tallerPieces.Count > widerPieces.Count)
            {
                widerPieces.Add(tallerPieces[0]);
                tallerPieces.Remove(tallerPieces[0]);
            }
            else if (tallerPieces.Count < widerPieces.Count)
            {
                tallerPieces.Add(widerPieces[0]);
                widerPieces.Remove(widerPieces[0]);
            }
        }

        placeLocation currentPlaceLocation = placeLocation.LEFT;

        float leftHeight = 0;
        float rightHeight = puzzleSize - 1;
        float topWidth = 0;
        float bottomWidth = puzzleSize - 1;

        for (int i = 0; i < widerPieces.Count; i++)
        {
            Vector2 minExtents = widerPieces[i].GetComponent<PieceProperties>().GetMinExtents();
            Vector2 maxExtents = widerPieces[i].GetComponent<PieceProperties>().GetMaxExtents();
            Vector2 currenPos = new Vector2(widerPieces[i].transform.position.x, widerPieces[i].transform.position.z);

            if (currentPlaceLocation == placeLocation.LEFT)
            {
                float newX = -2 - (maxExtents.x - currenPos.x);
                float newY = leftHeight + (currenPos.y - minExtents.y);
                widerPieces[i].transform.position = new Vector3(newX, 0, newY);
                widerPieces[i].name = i + "w-" + widerPieces[i].name;
                leftHeight += (maxExtents.y - minExtents.y) + 2;
                currentPlaceLocation = placeLocation.RIGHT;
            }
            else if (currentPlaceLocation == placeLocation.RIGHT)
            {
                float newX = puzzleSize + 1 + (currenPos.x - minExtents.x);
                float newY = rightHeight - (maxExtents.y - currenPos.y);
                widerPieces[i].transform.position = new Vector3(newX, 0, newY);
                widerPieces[i].name = i + "w-" + widerPieces[i].name;
                rightHeight -= (maxExtents.y - minExtents.y + 2);
                currentPlaceLocation = placeLocation.LEFT;
            }
        }

        currentPlaceLocation = placeLocation.TOP;

        for (int i = 0; i < tallerPieces.Count; i++)
        {
            Vector2 minExtents = tallerPieces[i].GetComponent<PieceProperties>().GetMinExtents();
            Vector2 maxExtents = tallerPieces[i].GetComponent<PieceProperties>().GetMaxExtents();
            Vector2 currenPos = new Vector2(tallerPieces[i].transform.position.x, tallerPieces[i].transform.position.z);

            if (currentPlaceLocation == placeLocation.TOP)
            {
                float newX = topWidth + (currenPos.x - minExtents.x);
                float newY = puzzleSize + 1 + (currenPos.y - minExtents.y);
                tallerPieces[i].transform.position = new Vector3(newX, 0, newY);
                tallerPieces[i].name = i + "t-" + tallerPieces[i].name;
                topWidth += (maxExtents.x - minExtents.x + 2);
                currentPlaceLocation = placeLocation.BOTTOM;
            }
            else if (currentPlaceLocation == placeLocation.BOTTOM)
            {
                float newX = bottomWidth - (maxExtents.x - currenPos.x);
                float newY = -2 - (maxExtents.y - currenPos.y);
                tallerPieces[i].transform.position = new Vector3(newX, 0, newY);
                tallerPieces[i].name = i + "t-" + tallerPieces[i].name;
                bottomWidth -= (maxExtents.x - minExtents.x + 2);
                currentPlaceLocation = placeLocation.TOP;
            }
        }

        float leftMost = 0;
        float rightMost = puzzleSize;
        float bottomMost = 0;
        float topMost = puzzleSize;
        foreach (GameObject piece in moveablePieces)
        {
            Vector2 minExtents = piece.GetComponent<PieceProperties>().GetMinExtents();
            Vector2 maxExtents = piece.GetComponent<PieceProperties>().GetMaxExtents();
            if(minExtents.x < leftMost)
            {
                leftMost = minExtents.x;
            }
            if (minExtents.y < bottomMost)
            {
                bottomMost = minExtents.y;
            }
            if (maxExtents.x > rightMost)
            {
                rightMost = minExtents.x;
            }
            if (maxExtents.y < topMost)
            {
                topMost = maxExtents.y;
            }
        }

        float width = topMost - bottomMost;
        float height = rightMost - leftMost;
        float size = Mathf.Max(width, height);

        ground.transform.position = new Vector3(puzzleSize / 2, -.02f, puzzleSize / 2);
        ground.transform.localScale = new Vector3(size / 5f + 1, 1, size / 5f + 1);

        sc.RegisterPieces(moveablePieces, puzzleSize);
    }
}
