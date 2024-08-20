using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPart : MonoBehaviour
{
    [SerializeField] Brick brick;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField]

    public void Initialize()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void ChangeMaterial(Material material)
    {
        if (meshRenderer == null)
        {
            Debug.Log("meshRender is null", gameObject);
            return;
        }

        if (material == null)
        {
            Debug.Log("Material is null", gameObject);
            return;
        }
        meshRenderer.material = material;
    }
}
