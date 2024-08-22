using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> parts;
    [SerializeField] float explodeForce;
    [Tooltip("True if the parts explode outward from the center, false to explode inward")]
    [SerializeField] bool outward = true;
    Vector3 center;

    public void Play()
    {
        center = transform.position;

        // make every part of this game object explode base on direction between the part and the game object 
        foreach (GameObject part in parts)
        {
            Vector3 direction = GetExplodeDirection(part.transform);

            IExplodable target = part.GetComponent<IExplodable>();
            if (target != null)
            {
                target.Explode(direction * explodeForce);
            }
        }
    }

    Vector3 GetExplodeDirection(Transform target)
    {
        if (outward)
        {
            return (target.position - center).normalized;
        }
        else
        {
            return (center - target.position).normalized;
        }
    }
}
