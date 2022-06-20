using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShower : MonoBehaviour
{
    SolutionChecker sc;
    GameObject menu;
    bool menuShowing = false;

    private void Awake()
    {
        sc = GameObject.FindGameObjectWithTag("GameController").GetComponent<SolutionChecker>();
        menu = transform.parent.GetChild(0).gameObject;
    }

    // Update is called once per frame
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
    }

    public void ShowMenu(bool show)
    {
        menu.SetActive(show);
    }
}
