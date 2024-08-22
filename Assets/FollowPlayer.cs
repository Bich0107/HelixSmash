using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;

    void Start() {
        offset = transform.position - target.position;
    }

    void LateUpdate() {
        transform.position = target.position + offset;
    }
}
