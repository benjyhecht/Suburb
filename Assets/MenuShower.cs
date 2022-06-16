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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
