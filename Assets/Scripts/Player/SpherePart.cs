using UnityEngine;

public class SpherePart : MonoBehaviour, IExplodable
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Collider bodyCollider;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bodyCollider = GetComponentInChildren<Collider>();

        bodyCollider.enabled = false;
    }

    public void Explode(Vector3 force)
    {
        bodyCollider.enabled = true;

        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce(force, ForceMode.Impulse);
    }
}
