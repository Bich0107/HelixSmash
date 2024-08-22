using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotateAnimation))]
public class Brick : MonoBehaviour
{
    static float s_explodeForce = 15f;
    static float s_deactiveDelay = 1.5f;

    [SerializeField] List<BrickPart> parts = new List<BrickPart>();
    AudioClip breakingSound;
    RotateAnimation rotater;
    int specialPartAmount;
    bool breaked;

    public void Initialize(float partRatio, Material partMaterial, AudioClip clip)
    {
        rotater = GetComponent<RotateAnimation>();

        SpecialPartRatio = partRatio;
        SpecialMaterial = partMaterial;

        // the amount of special part base on the total amount of part
        specialPartAmount = Mathf.RoundToInt(transform.childCount * SpecialPartRatio);

        breakingSound = clip;
    }

    public void SetRotation(float angle)
    {
        rotater.Initialize(new Vector3(0f, angle, 0f), 0f, true);
    }

    // choose parts to add special material for them, type 1 is select alternately
    public void SetPartsMaterialType1()
    {
        int counter = 0;

        // change part material every divider part (min 2)
        int divider = Mathf.Max(2, transform.childCount / specialPartAmount);

        foreach (Transform child in transform)
        {
            BrickPart part = child.GetComponent<BrickPart>();
            part.Initialize(this);

            // check and set up part if it is a special part
            if (counter % divider == 0)
            {
                part.ChangeMaterial(SpecialMaterial);
                part.SetSpecialPart();
            }

            counter++;

            parts.Add(part);
        }
    }

    // choose parts to add special material for them, type 2 is select continuously
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

    // call when a part of the brick is trigger by ball
    public void Break(Ball ball)
    {
        if (breaked) return;
        breaked = true;

        AudioManager.Instance.PlaySound(breakingSound);

        rotater.Stop(); // stop rotating

        ball.IncreaseCounter(); // increase destroyed brick counter

        foreach (BrickPart part in parts)
        {
            if (part.gameObject == null) continue;

            IExplodable target = part.GetComponent<IExplodable>();
            if (target != null)
            {
                target.Explode(s_explodeForce * part.transform.forward);
            }
        }

        StartCoroutine(CR_Deactive());
    }

    IEnumerator CR_Deactive()
    {
        yield return new WaitForSeconds(s_deactiveDelay);
        Destroy(gameObject);
    }

    public float SpecialPartRatio { get; set; }
    public Material SpecialMaterial { get; set; }
}
