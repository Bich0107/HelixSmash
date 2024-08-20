using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPart : MonoBehaviour
{
    [SerializeField] Brick brick;
    [SerializeField] MeshRenderer meshRenderer;
    bool isSpecialPart;

    public void Initialize()
    {
        brick = GetComponentInParent<Brick>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTrigger(other);
        }
    }

    void PlayerTrigger(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) return;

        if (ball.IsAttacking)
        {
            if (ball.IsInvincible || !isSpecialPart)
            {
                ball.IncreaseCounter();
                brick.Break();
            }
            else
            {
                Debug.Log("game over");
                ball.Dead();
            }
        }
        else
        {
            ball.BounceUp();
        }
    }

    public void ChangeMaterial(Material material)
    {
        if (meshRenderer == null) return;

        meshRenderer.material = material;
    }

    public void SetSpecialPart() => isSpecialPart = true;
}
