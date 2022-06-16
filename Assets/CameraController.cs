using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Vector3 lookLocation = new Vector3(0, 0, 0);
    float lookDistance = 10f;
    float minLookDistance = 5f;
    float maxLookDistance = 20f;
    float scrollScale = 2f;
    bool moving = false;

    float minVAngle = 20;
    float maxVAngle = 60;
    float vAngle = 20;
    float hAngle = 0;

    Vector2 originalMousePosition;
    Vector2 mousePosition;
    float vFactor = .15f;
    float hFactor = .2f;
    float hDistance;
    float rotationSpeed = 5f;

    SolutionChecker sc;

    void Awake()
    {
        sc = GameObject.FindGameObjectWithTag("GameController").GetComponent<SolutionChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sc.GetPlayable())
        {
            if (!(Input.GetKey(KeyCode.Mouse0)) && Input.GetKey(KeyCode.Mouse1))
            {
                if (!moving)
                {
                    originalMousePosition = Input.mousePosition;
                    moving = true;
                }
                else
                {
                    mousePosition = Input.mousePosition;
                    mousePosition -= originalMousePosition;
                    vAngle -= mousePosition.y * vFactor;
                    hAngle -= mousePosition.x * hFactor;
                    if (vAngle > maxVAngle)
                    {
                        vAngle = maxVAngle;
                    }
                    if (vAngle < minVAngle)
                    {
                        vAngle = minVAngle;
                    }
                    originalMousePosition = Input.mousePosition;
                }
            }
            else
            {
                if (moving)
                {
                    moving = false;
                }
            }

            lookDistance -= Input.mouseScrollDelta.y * scrollScale;
            lookDistance = Mathf.Clamp(lookDistance, minLookDistance, maxLookDistance);

            float y = lookDistance * Mathf.Sin(vAngle * Mathf.Deg2Rad);
            hDistance = lookDistance * Mathf.Cos(vAngle * Mathf.Deg2Rad);
            float x = hDistance * Mathf.Cos(hAngle * Mathf.Deg2Rad);
            float z = hDistance * Mathf.Sin(hAngle * Mathf.Deg2Rad);

            transform.position = (new Vector3(x, y, z)) + lookLocation;
            transform.LookAt(lookLocation);
        }
        else
        {
            transform.RotateAround(lookLocation, Vector3.up, Time.deltaTime * rotationSpeed);
        }
    }

    public void ChangeLookLocation(Vector3 newLocation, float newDistance)
    {
        lookLocation = newLocation;
        lookDistance = newDistance;
        minLookDistance = lookDistance / 2f;
        maxLookDistance = lookDistance * 1.5f;
    }

    public void EndGame()
    {
        lookDistance = minLookDistance;
        vAngle = minVAngle;
        float y = lookDistance * Mathf.Sin(vAngle * Mathf.Deg2Rad);
        hDistance = lookDistance * Mathf.Cos(vAngle * Mathf.Deg2Rad);
        float x = hDistance * Mathf.Cos(hAngle * Mathf.Deg2Rad);
        float z = hDistance * Mathf.Sin(hAngle * Mathf.Deg2Rad);
        StartCoroutine(MoveCamera((new Vector3(x, y, z)) + lookLocation));       
    }

    IEnumerator MoveCamera(Vector3 newLocation)
    {
        float timeTaken = 0;
        float timeToTake = .5f;
        Vector3 originalPosition = transform.position;
        while (timeTaken < timeToTake)
        {
            transform.position = Vector3.Lerp(originalPosition, newLocation, timeTaken / timeToTake);
            transform.LookAt(lookLocation);
            timeTaken += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = newLocation;
        transform.LookAt(lookLocation);
        yield return new WaitForEndOfFrame();
    }
}
