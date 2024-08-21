using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float gravity;
    [SerializeField] Vector3 direction;

    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        rigid.velocity -= direction * gravity * Time.fixedDeltaTime;
    }
}
