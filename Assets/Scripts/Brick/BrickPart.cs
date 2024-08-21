using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickPart : MonoBehaviour, IExplodable
{
    [SerializeField] Brick brick;
    [SerializeField] Collider bodyCollider;
    [SerializeField] Rigidbody rigid;
    [SerializeField] MeshRenderer meshRenderer;
    bool isSpecialPart;
    bool attackTriggered;

    public void Initialize(Brick _brick)
    {
        brick = _brick;
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        rigid = GetComponent<Rigidbody>();
        bodyCollider = GetComponentInChildren<Collider>();

        rigid.isKinematic = true;
        rigid.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Triggered(other);
        }
    }

    void Triggered(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null || ball.IsFinish || attackTriggered)
        {
            return;
        }

        if (ball.IsAttacking)
        {
            attackTriggered = true;
            if (isSpecialPart)
            {
                if (ball.IsInvincible)
                {
                    ball.IncreaseInvincibleTime();
                    brick.Break();
                    gameObject.SetActive(false);
                }
                else
                {
                    ball.Die();
                }
            }
            else
            {
                ball.IncreaseCounter();
                brick.Break();
                gameObject.SetActive(false);
            }
        }
        else
        {
            ball.BounceUp();
        }
    }

    public void Explode(Vector3 force)
    {
        bodyCollider.isTrigger = false;

        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce(force, ForceMode.Impulse);
    }

    public void ChangeMaterial(Material material)
    {
        if (meshRenderer == null) return;

        meshRenderer.material = material;
    }

    public void SetSpecialPart() => isSpecialPart = true;
}
