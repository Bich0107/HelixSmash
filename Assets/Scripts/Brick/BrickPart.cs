using UnityEngine;

public class BrickPart : MonoBehaviour, IExplodable
{
    [SerializeField] Brick brick;
    [SerializeField] Collider bodyCollider;
    [SerializeField] Rigidbody rigid;
    [SerializeField] MeshRenderer meshRenderer;
    bool isSpecialPart;
    bool triggered;

    public void Initialize(Brick _brick)
    {
        brick = _brick;
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        rigid = GetComponent<Rigidbody>();
        bodyCollider = GetComponentInChildren<Collider>();

        // disable physics
        rigid.isKinematic = true;
        rigid.useGravity = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Triggered(other);
        }
    }

    void Triggered(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null || ball.IsFinish || triggered || ball.IsDead)
        {
            return;
        }

        if (ball.IsAttacking)
        {
            triggered = true; // turn on flag
            if (isSpecialPart)
            {
                 // if player is invincible, increase invincible time and tell the brick to break and disappear
                if (ball.IsInvincible)
                {
                    ball.IncreaseInvincibleTime();
                    brick.Break(ball);
                    Disappear();
                }
                else // if not, kill player
                {
                    ball.Die();
                    GameManager.Instance.GameOver();
                }
            }
            else
            {
                brick.Break(ball);
                Disappear();
            }
        }
        else
        {
            ball.BounceUp();
        }
    }

    void Disappear()
    {
        meshRenderer.enabled = false;
    }

    public void Explode(Vector3 force)
    {
        // turn on physics and collider
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
