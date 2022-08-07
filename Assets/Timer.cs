using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    float time;
    string minutes;
    string seconds;
    Text text;
    TextMeshProUGUI textMeshPro;
    bool timeStarted;
    bool timeCurrentlyRunning;
    bool timeEnded;

    void Start()
    {
        time = 0;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        timeStarted = false;
        timeCurrentlyRunning = false;
        timeEnded = false;
    }

    void Update()
    {
        if (timeStarted && timeCurrentlyRunning && !timeEnded)
        {
            time += Time.deltaTime;
            minutes = Mathf.Floor(time / 60).ToString("00");
            seconds = (time % 60).ToString("00");
            textMeshPro.text = minutes + ":" + seconds;
        }
    }

    public void startTime()
    {
        print("Time started");
        if (timeStarted)
        {
            return;
        }
        timeStarted = true;
        timeCurrentlyRunning = true;
    }

    public void pauseTime(bool pause)
    {
        if (timeStarted && !timeEnded)
        {
            timeCurrentlyRunning = pause;
        }
    }

    public void endTime()
    {
        timeEnded = true;
    }
}
