using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShower : MonoBehaviour
{
    SolutionChecker sc;
    GameObject menu;
    bool menuShowing = false;
    Timer timer;

    private void Awake()
    {
        sc = GameObject.FindGameObjectWithTag("GameController").GetComponent<SolutionChecker>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        menu = transform.parent.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ButtonShowMenu();
        }
    }

    public void ButtonShowMenu()
    {
        menuShowing = !menuShowing;
        ShowMenu(menuShowing);
        sc.SetPaused(menuShowing);
        timer.pauseTime(!menuShowing);
    }

    public void ShowMenu(bool show)
    {
        menu.SetActive(show);
    }
}
