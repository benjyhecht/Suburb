using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseShower : MonoBehaviour
{
    [SerializeField] LayerMask prefabsLayer;
    [SerializeField] LayerMask movementPlaneLayer;
    Camera mainCamera;
    GameObject newMouseOverObject = null;
    GameObject oldMouseOverObject = null;
    Vector3 newPosition;

    bool holdingObject = false;

    SolutionChecker sc;

    void Awake()
    {
        mainCamera = Camera.main;
        sc = GetComponent<SolutionChecker>();
    }

    void Update()
    {
        if (sc.GetPlayable())
        {
            PerformRaycast();
            if (newMouseOverObject == null && oldMouseOverObject == null)
            {
                //Do nothing
            }
            else if (newMouseOverObject != null && oldMouseOverObject == null)
            {
                if (newMouseOverObject.transform.parent.GetComponent<PieceProperties>())
                {
                    newMouseOverObject.transform.parent.GetComponent<PieceProperties>().LightenPiece(Enums.Highlights.ON);
                }
            }
            else if (newMouseOverObject == null && oldMouseOverObject != null)
            {
                if (oldMouseOverObject.transform.parent.GetComponent<PieceProperties>())
                {
                    oldMouseOverObject.transform.parent.GetComponent<PieceProperties>().LightenPiece(Enums.Highlights.OFF);
                }
            }
            else
            {
                if (newMouseOverObject != null && oldMouseOverObject != null)
                {
                    if (newMouseOverObject.transform.parent != oldMouseOverObject.transform.parent)
                    {
                        if (newMouseOverObject.transform.parent.GetComponent<PieceProperties>())
                        {
                            newMouseOverObject.transform.parent.GetComponent<PieceProperties>().LightenPiece(Enums.Highlights.ON);
                        }
                        if (oldMouseOverObject.transform.parent.GetComponent<PieceProperties>())
                        {
                            oldMouseOverObject.transform.parent.GetComponent<PieceProperties>().LightenPiece(Enums.Highlights.OFF);
                        }
                    }
                }
            }

            oldMouseOverObject = newMouseOverObject;
        }
    }

    private void PerformRaycast()
    {
        RaycastHit prefabHit;
        RaycastHit planeHit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!holdingObject)
        {
            if (Physics.Raycast(ray, out prefabHit, 1000f, ~movementPlaneLayer))
            {
                newMouseOverObject = prefabHit.transform.gameObject;
            }
            else
            {
                newMouseOverObject = null;
            }
        }

        if (Physics.Raycast(ray, out planeHit, 1000f, ~prefabsLayer))
        {
            newPosition = planeHit.point;
        }
    }

    public Vector3 GetMousePosition()
    {
        return newPosition;
    }

    public bool GetHoldingObject()
    {
        return holdingObject;
    }

    public void SetHoldingObject(bool holdingObject)
    {
        this.holdingObject = holdingObject;
    }

}
