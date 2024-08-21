using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickPart : MonoBehaviour
{
    [SerializeField] Brick brick;
    [SerializeField] MeshRenderer meshRenderer;
    bool isSpecialPart;

    public void Initialize(Brick _brick)
    {
        brick = _brick;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
        if (ball == null || ball.IsFinish) {
            return;
        }

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
                SceneManager.LoadScene(0);
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
