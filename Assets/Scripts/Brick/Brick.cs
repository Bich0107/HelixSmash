using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateAnimation))]
public class Brick : MonoBehaviour
{
    static float s_explodeForce = 15f;
    static float s_deactiveDelay = 1.5f;

    [SerializeField] List<BrickPart> parts = new List<BrickPart>();
    RotateAnimation rotater;
    int specialPartAmount;
    bool breaked;

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
            part.Initialize(this);

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
            part.Initialize(this);

            if (counter < specialPartAmount)
            {
                part.ChangeMaterial(SpecialMaterial);
                part.SetSpecialPart();
            }

            counter++;

            parts.Add(part);
        }
    }

    public void Break(Ball ball)
    {
        if (breaked) return;
        breaked = true;

        rotater.Stop();

        ball.IncreaseCounter();

        foreach (BrickPart part in parts) {
            if (part.gameObject == null) continue;

            IExplodable target = part.GetComponent<IExplodable>();
            if (target != null) {
                target.Explode(s_explodeForce * part.transform.forward);
            }
        }

        StartCoroutine(CR_Deactive());
    }

    IEnumerator CR_Deactive() {
        yield return new WaitForSeconds(s_deactiveDelay);
        Destroy(gameObject);
    }

    public float SpecialPartRatio { get; set; }
    public Material SpecialMaterial { get; set; }
}
