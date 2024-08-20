using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float bounceHeight;
    [SerializeField] float bounceTime;
    [SerializeField] float bounceDistance;
    bool isBouncing;
    [Header("Attack settings")]
    [SerializeField] float baseSpeed;
    [SerializeField] float dropSpeed;
    [SerializeField] float invincibleTime;
    [Tooltip("Continuously destroy brickAmount brick to be invincible for invincibleTime")]
    [SerializeField] int brickAmount;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isInvincible = false;
    int brickCounter;
    float invincibleTimer;
    public bool IsAttacking => isAttacking;
    public bool IsInvincible => isInvincible;

    bool isDead = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetMouseButton(0))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    public void IncreaseCounter()
    {
        brickCounter++;
        if (brickCounter >= brickAmount)
        {
            StartCoroutine(CR_Invincible());
        }
    }

    IEnumerator CR_Invincible()
    {
        isInvincible = true;
        invincibleTimer = 0f;

        while (invincibleTimer <= invincibleTime)
        {
            invincibleTimer += Time.deltaTime;
            yield return null;
        }

        isInvincible = false;
    }

    public void BounceUp()
    {
        if (isBouncing) return;

        isBouncing = true;
        ToggleGravity();
        bounceHeight = transform.position.y;
        StartCoroutine(CR_BounceUp());
    }

    IEnumerator CR_BounceUp()
    {
        float targetHeight = bounceHeight + bounceDistance;

        float tick = 0f;
        float startHeight = transform.position.y;
        Vector3 newPosition = transform.position;
        while (!Mathf.Approximately(transform.position.y, targetHeight))
        {
            tick += Time.deltaTime;

            newPosition.y = Mathf.Lerp(startHeight, targetHeight, tick / bounceTime);
            transform.position = newPosition;
            yield return null;
        }

        newPosition.y = targetHeight;
        transform.position = newPosition;

        rigid.velocity = Vector3.zero;
        ToggleGravity();

        isBouncing = false;
    }

    public void Dead() {
        isDead = true;
        StopAllCoroutines();
        rigid.useGravity = false;
        isAttacking = false;
        isInvincible = false;
    }

    void ToggleGravity()
    {
        if (rigid == null) return;

        rigid.useGravity = !rigid.useGravity;
    }
}
