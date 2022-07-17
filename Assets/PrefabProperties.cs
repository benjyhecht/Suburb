using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabProperties : MonoBehaviour
{
    struct MatList
    {
        internal Transform transform;
        internal Material[] darkMats;
        internal Material[] lightMats;
        internal Material[] greenMats;
        internal Material[] redMats;
    }

    [SerializeField] Enums.Environment environment;
    [SerializeField] Enums.BlockType blockType;
    [SerializeField] int matChange;

    List<MatList> matList = new List<MatList>();

    private void Awake()
    {

    }

    public void OnEnable()
    {
        if (environment == Enums.Environment.NA && blockType == Enums.BlockType.NA)
        {
            this.enabled = false;
        }
    }

    public Enums.Environment GetEnvironment()
    {
        return environment;
    }

    public Enums.BlockType GetBlockType()
    {
        return blockType;
    }

    public int GetMatChange()
    {
        return matChange;
    }

    public void CalculateColors()
    {
        AddToMatList(transform);

        foreach (Transform child in transform)
        {
            AddToMatList(child);
        }
    }

    public void LightenBlock(Enums.Highlights color)
    {
        foreach(MatList tempMatList in matList)
        {
            LightComponent(tempMatList, color);
        }
    }

    public void SetInitialColor(Color color)
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;
        if (mats.Length - 1 >= matChange)
        {
            mats[matChange].color = color;
        }
        else
        {
            foreach (Transform child in transform)
            {
                Material[] childMats = child.GetComponent<MeshRenderer>().materials;
                foreach (Material childMat in childMats)
                {
                    if (childMat.name.Contains("Brick"))
                    {
                        childMat.color = color;
                    }
                }
            }
        }
    }

    private void LightComponent(MatList tempMatList, Enums.Highlights color)
    {
        switch (color)
        {
            case Enums.Highlights.ON:
                tempMatList.transform.GetComponent<MeshRenderer>().materials = tempMatList.lightMats;
                break;
            case Enums.Highlights.OFF:
                tempMatList.transform.GetComponent<MeshRenderer>().materials = tempMatList.darkMats;
                break;
            case Enums.Highlights.GREEN:
                tempMatList.transform.GetComponent<MeshRenderer>().materials = tempMatList.greenMats;
                break;
            case Enums.Highlights.RED:
                tempMatList.transform.GetComponent<MeshRenderer>().materials = tempMatList.redMats;
                break;
            default:
                tempMatList.transform.GetComponent<MeshRenderer>().materials = tempMatList.darkMats;
                break;
        }
    }

    private void AddToMatList(Transform transform)
    {
        try
        {
            Material[] darkMats = transform.GetComponent<MeshRenderer>().materials;
            Material[] lightMats = new Material[darkMats.Length];
            Material[] greenMats = new Material[darkMats.Length];
            Material[] redMats = new Material[darkMats.Length];
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i] = Instantiate(darkMats[i]);
                greenMats[i] = Instantiate(darkMats[i]);
                redMats[i] = Instantiate(darkMats[i]);
                if (!lightMats[i].name.Contains("Grass"))
                {
                    Color lightColor = new Color(darkMats[i].color.r * 1.5f, darkMats[i].color.g * 1.5f, darkMats[i].color.b * 2.5f);
                    Color greenColor = new Color(darkMats[i].color.r * 1f, darkMats[i].color.g * 2.5f, darkMats[i].color.b * 2f);
                    Color redColor = new Color(darkMats[i].color.r * 2f, darkMats[i].color.g * 1.5f, darkMats[i].color.b * 1.5f);
                    lightMats[i].color = lightColor;
                    greenMats[i].color = greenColor;
                    redMats[i].color = redColor;
                }
            }

            MatList tempMatList = new MatList();
            tempMatList.transform = transform;
            tempMatList.darkMats = darkMats;
            tempMatList.lightMats = lightMats;
            tempMatList.greenMats = greenMats;
            tempMatList.redMats = redMats;
            matList.Add(tempMatList);
        }
        catch (Exception e)
        {
            print(e);
        }
    }
}
