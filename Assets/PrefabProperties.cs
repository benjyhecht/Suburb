using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabProperties : MonoBehaviour
{
    [SerializeField] Enums.Environment environment;
    [SerializeField] Enums.BlockType blockType;
    [SerializeField] int matChange;

    Material[] lightMats;
    Material[] darkMats;
    Material[] greenMats;

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
        darkMats = GetComponent<MeshRenderer>().materials;
        lightMats = new Material[darkMats.Length];
        greenMats = new Material[darkMats.Length];
        for (int i = 0; i < lightMats.Length; i++)
        {
            lightMats[i] = Instantiate(darkMats[i]);
            greenMats[i] = Instantiate(darkMats[i]);
            if (!lightMats[i].name.Contains("Grass"))
            {
                Color lightColor = new Color(darkMats[i].color.r * 1.5f, darkMats[i].color.g * 1.5f, darkMats[i].color.b * 2.5f);
                Color greenColor = new Color(darkMats[i].color.r * 1f, darkMats[i].color.g * 2.5f, darkMats[i].color.b * 2f);
                lightMats[i].color = lightColor;
                greenMats[i].color = greenColor;
            }
        }
    }

    public void LightenBlock(Enums.Highlights color)
    {
        switch (color)
        {
            case Enums.Highlights.ON:
                GetComponent<MeshRenderer>().materials = lightMats;
                break;
            case Enums.Highlights.OFF:
                GetComponent<MeshRenderer>().materials = darkMats;
                break;
            case Enums.Highlights.GREEN:
                GetComponent<MeshRenderer>().materials = greenMats;
                break;
            default:
                GetComponent<MeshRenderer>().materials = darkMats;
                break;
        }
    }
}
