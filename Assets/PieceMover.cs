using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
    private bool lookedAt = false;
    private bool moving = false;
    private float mZCoord;
    private Vector3 mOffset;
    private MouseShower mouseShower;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private int originalLayer;
    private SolutionChecker sc;
    private PieceProperties pieceProperties;
    private DontDestroy dd;
    private SFXManager sfxManager;
    private Vector2 minExtents;
    private Vector2 maxExtents;
    private int puzzleSize;
    bool green = false;

    private void Awake()
    {
        mouseShower = GameObject.FindGameObjectWithTag("GameController").GetComponent<MouseShower>();
        sc = mouseShower.gameObject.GetComponent<SolutionChecker>();
        dd = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
        pieceProperties = GetComponent<PieceProperties>();
        sfxManager = mouseShower.GetComponent<SFXManager>();
    }

    private void Update()
    {
        if (lookedAt && sc.GetPlayable())
        {
            if (Input.GetMouseButtonDown(0) && !moving)
            {
                moving = true;
                mouseShower.SetHoldingObject(true);
                StartMoving();
            }

            if (Input.GetMouseButton(0) && moving)
            {
                KeepMoving();
            }

            if (dd.GetRotationAllowed() && Input.GetMouseButtonUp(1) && moving)
            {
                RotatePiece();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopMoving();
                moving = false;
                mouseShower.SetHoldingObject(false);
            }
        }

        if (moving && !lookedAt)
        {
            StopMoving();
            moving = false;
            mouseShower.SetHoldingObject(false);
        }
    }

    public bool GetLookedAt()
    {
        return lookedAt;
    }

    public void SetLookedAt(bool lookedAt)
    {
        this.lookedAt = lookedAt;
    }

    public void StartMoving()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mOffset = gameObject.transform.position - mouseShower.GetMousePosition() + new Vector3(0, .1f, 0);
        sfxManager.PlaySound(Enums.SFX.PICKUP);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void KeepMoving()
    {
        transform.position = mouseShower.GetMousePosition() + mOffset;
    }

    public void RotatePiece()
    {
        Vector2 midPoint = (pieceProperties.GetMaxExtents() + pieceProperties.GetMinExtents()) / 2;
        //transform.RotateAround(new Vector3(midPoint.x, 0, midPoint.y), Vector3.up, 90);
        transform.RotateAround(mouseShower.GetMousePosition(), Vector3.up, 90);
    }

    public void StopMoving()
    {
        green = false;
        if (SafeToLand())
        {
            pieceProperties.LightenPiece(Enums.Highlights.ON);
            Vector3 exactPosition = transform.position;
            float roundX = Mathf.RoundToInt(exactPosition.x);
            float roundZ = Mathf.RoundToInt(exactPosition.z);
            transform.position = new Vector3(roundX, 0, roundZ);
            sc.CheckGameOver();
        }
        else
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;
        }
        sfxManager.PlaySound(Enums.SFX.PUTDOWN);
    }

    public bool SafeToLand()
    {
        foreach (Transform child in transform)
        {
            Vector3 exactPosition = child.transform.position;
            float roundX = Mathf.RoundToInt(exactPosition.x);
            float roundZ = Mathf.RoundToInt(exactPosition.z);
            Vector3 possiblePosition = new Vector3(roundX, exactPosition.y, roundZ) + Vector3.up;

            RaycastHit[] prefabHit;

            prefabHit = Physics.RaycastAll(possiblePosition, Vector3.down, 5f);
            //print("Objects hit: " + prefabHit.Length);
            if (prefabHit.Length > 2)
            {
                return false;
            }
        }

        return true;
    }
}
