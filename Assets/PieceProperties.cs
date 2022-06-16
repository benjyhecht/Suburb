using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceProperties : MonoBehaviour
{
    Vector2 pieceMinExtents;
    Vector2 pieceMaxExtents;

    public void LightenPiece(Enums.Highlights color)
    {
        foreach(Transform block in transform)
        {
            block.GetComponent<PrefabProperties>().LightenBlock(color);
        }
        if (color == Enums.Highlights.OFF)
        {
            GetComponent<PieceMover>().SetLookedAt(false);
        }
        else
        {
            GetComponent<PieceMover>().SetLookedAt(true);
        }
    }

    public void CalculateExtents()
    {
        float minX = transform.GetChild(0).transform.position.x;
        float maxX = transform.GetChild(0).transform.position.x;
        float minY = transform.GetChild(0).transform.position.z;
        float maxY = transform.GetChild(0).transform.position.z;
        foreach (Transform child in transform)
        {
            float tempX = child.position.x;
            float tempY = child.position.z;
            if (tempX > maxX)
            {
                maxX = tempX;
            }
            if (tempX < minX)
            {
                minX = tempX;
            }
            if (tempY > maxY)
            {
                maxY = tempY;
            }
            if (tempY < minY)
            {
                minY = tempY;
            }
        }

        pieceMinExtents = new Vector2(minX, minY);
        pieceMaxExtents = new Vector2(maxX, maxY);

        this.gameObject.name = pieceMaxExtents.ToString() + " " +pieceMinExtents.ToString();
    }

    public Vector2 GetMinExtents()
    {
        return pieceMinExtents;
    }

    public Vector2 GetMaxExtents()
    {
        return pieceMaxExtents;
    }

    public Vector2 GetExtents()
    {
        return (pieceMaxExtents - pieceMinExtents);
    }
}
