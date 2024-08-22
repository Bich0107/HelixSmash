using UnityEngine;

public class SpherePart : MonoBehaviour, IExplodable
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Collider bodyCollider;
    Vector3 basePos;
    Quaternion baseRotation;
    Vector3 baseScale;

    void Awake()
    {
        basePos = transform.localPosition;
        baseRotation = transform.localRotation;
        baseScale = transform.localScale;
    }

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

    public void Reset()
    {
        bodyCollider.enabled = false;

        rigid.isKinematic = true;
        rigid.useGravity = false;

        transform.localPosition = basePos;
        transform.localRotation = baseRotation;
        transform.localScale = baseScale;
    }
}
