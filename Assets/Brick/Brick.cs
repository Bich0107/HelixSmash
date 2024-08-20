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

        SetPartsMaterial();
    }

    public void SetRotation(float angle)
    {
        rotater.Initialize(new Vector3(0f, angle, 0f), 0f, true);
    }

    public void SetPartsMaterial()
    {
        int counter = 0;
        foreach (Transform child in transform)
        {
            BrickPart part = child.GetComponent<BrickPart>();
            part.Initialize();

            if (counter < specialPartAmount)
            {
                part.ChangeMaterial(SpecialMaterial);
            }

            counter++;

            parts.Add(part);
        }
    }

    public float SpecialPartRatio { get; set; }
    public Material SpecialMaterial { get; set; }
}
