using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuMessageManager : MonoBehaviour
{
    private void OnEnable()
    {
        SolutionChecker sc = GameObject.FindGameObjectWithTag("GameController").GetComponent<SolutionChecker>();
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        if (sc.GetGameOver())
        {
            textMeshPro.text = "Congratulations!";
        }
        else
        {
            textMeshPro.text = "Paused ...";
        }
    }
}
