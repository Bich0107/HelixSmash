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
    [SerializeField] float dropSpeed;
    [SerializeField] float invincibleTime;
    [Tooltip("Continuously destroy brickAmount brick to be invincible for invincibleTime")]
    [SerializeField] int brickAmount;
    int brickCounter;
    float invincibleTimer;
    
    // flags
    bool isAttacking = false;
    bool isInvincible = false;
    bool isFinish;
    public bool IsFinish => isFinish;
    public bool IsAttacking => isAttacking;
    public bool IsInvincible => isInvincible;

    bool isDead = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDead || isFinish) return;

        ProcessMouseClick();
    }

    void ProcessMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        else
        {
            StopAttack();
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            StopAllCoroutines();

            rigid.useGravity = false;
            isBouncing = false;
            isAttacking = true;
            rigid.velocity = dropSpeed * Vector3.down;
        }
    }

    void StopAttack()
    {
        if (isAttacking)
        {
            StopAllCoroutines();

            isBouncing = false;
            isAttacking = false;
            isInvincible = false;
            rigid.velocity = Vector3.zero;
            rigid.useGravity = true;
        }
    }

    public void IncreaseCounter()
    {
        brickCounter++;
        if (brickCounter >= brickAmount && !isInvincible)
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
        SetGravity(false);
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
        SetGravity(true);

        isBouncing = false;
    }

    public void Dead()
    {
        isDead = true;
        Stop();
    }

    public void Finish()
    {
        isFinish = true;
        Stop();
    }

    public void Stop()
    {
        StopAllCoroutines();

        isBouncing = false;
        rigid.velocity = Vector3.zero;
        rigid.useGravity = false;
        isAttacking = false;
        isInvincible = false;
    }

    void SetGravity(bool status)
    {
        if (rigid == null) return;

        rigid.useGravity = status;
    }
}
