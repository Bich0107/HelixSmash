using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateAnimation))]
public class Brick : MonoBehaviour
{
    [SerializeField] List<BrickPart> parts = new List<BrickPart>();
    RotateAnimation rotater;
    int specialPartAmount;

    public void Initialize(float partRatio, Material partMaterial)
    {
        rotater = GetComponent<RotateAnimation>();

        SpecialPartRatio = partRatio;
        SpecialMaterial = partMaterial;

        // the amount of special part base on the total amount of part
        specialPartAmount = Mathf.RoundToInt(transform.childCount * SpecialPartRatio);
    }

    public void SetRotation(float angle)
    {
        rotater.Initialize(new Vector3(0f, angle, 0f), 0f, true);
    }

    public void SetPartsMaterialType1()
    {
        int counter = 0;

        // change part material every divider part (min 2)
        int divider = Mathf.Max(2, transform.childCount / specialPartAmount);

        foreach (Transform child in transform)
        {
            BrickPart part = child.GetComponent<BrickPart>();
            part.Initialize();

            if (counter % divider == 0)
            {
                part.ChangeMaterial(SpecialMaterial);
                part.SetSpecialPart();
            }

            counter++;

            parts.Add(part);
        }
    }

    public void SetPartsMaterialType2()
    {
        int counter = 0;

        foreach (Transform child in transform)
        {
            BrickPart part = child.GetComponent<BrickPart>();
            part.Initialize();

            if (counter < specialPartAmount)
            {
                part.ChangeMaterial(SpecialMaterial);
                part.SetSpecialPart();
            }

            counter++;

            parts.Add(part);
        }
    }

    public void Break()
    {
        // brick break
        Debug.Log("brick break", gameObject);
        gameObject.SetActive(false);
    }

    public float SpecialPartRatio { get; set; }
    public Material SpecialMaterial { get; set; }
}
