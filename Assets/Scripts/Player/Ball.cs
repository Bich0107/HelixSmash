using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] Vector3 basePos;
    [SerializeField] List<SpherePart> parts;
    [SerializeField] ExplodeAnimation explodeAnimation;
    [SerializeField] CounterDisplayer displayer;
    [SerializeField] ParticleSystem fireVFX;
    [Header("Movement settings")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float bounceHeight;
    [SerializeField] float bounceTime;
    [SerializeField] float bounceDistance;
    bool isBouncing;
    [Header("Attack settings")]
    [SerializeField] float dropSpeed;
    [SerializeField] float baseInvincibleTime = 3f;
    [SerializeField] float invincibleTime;
    [SerializeField] float timeIncreasePerDestroyedBrick = 0.001f;
    [Tooltip("Continuously destroy brickAmount brick to be invincible for invincibleTime")]
    [SerializeField] int brickAmount;
    [SerializeField] int brickCounter;
    public int BrickCounter => brickCounter;
    [SerializeField] int continuousCounter;
    [SerializeField] float invincibleTimer;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isInvincible = false;

    // flags
    bool isFinish;
    public bool IsFinish => isFinish;
    public bool IsAttacking => isAttacking;
    public bool IsInvincible => isInvincible;

    bool isDead = false;

    void Awake()
    {
        basePos = transform.position;
    }

    void Start()
    {
        explodeAnimation = GetComponent<ExplodeAnimation>();
        rigid = GetComponent<Rigidbody>();
        displayer = FindObjectOfType<CounterDisplayer>();

        displayer.Display(0);
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

            Stop(true);
        }
    }

    public void IncreaseInvincibleTime()
    {
        invincibleTime += timeIncreasePerDestroyedBrick;
    }

    public void IncreaseCounter()
    {
        brickCounter++;
        displayer.Display(brickCounter);
        continuousCounter++;
        if (continuousCounter >= brickAmount && !isInvincible)
        {
            StartCoroutine(CR_Invincible());
        }
    }

    IEnumerator CR_Invincible()
    {
        isInvincible = true;
        invincibleTimer = 0f;
        invincibleTime = baseInvincibleTime;
        continuousCounter = 0;

        SetInvincibleEffect(true);

        while (invincibleTimer <= invincibleTime)
        {
            invincibleTimer += Time.deltaTime;
            yield return null;
        }

        SetInvincibleEffect(false);

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

    public void Die()
    {
        isDead = true;
        Stop();

        explodeAnimation.Play();
    }

    public void Finish()
    {
        isFinish = true;

        Stop();
    }

    public void Stop(bool useGravity = false)
    {
        StopAllCoroutines();

        SetInvincibleEffect(false);
        continuousCounter = 0;
        isBouncing = false;
        rigid.velocity = Vector3.zero;
        isAttacking = false;
        isInvincible = false;
        SetGravity(useGravity);
    }

    public void Reset()
    {
        transform.position = basePos;
        isFinish = false;
        isDead = false;

        brickCounter = 0;

        foreach (SpherePart part in parts)
        {
            part.Reset();
        }

        displayer.Reset();

        Stop(true);
    }

    void SetInvincibleEffect(bool status)
    {
        ParticleSystem.EmissionModule emission = fireVFX.emission;
        emission.enabled = status;
    }

    void SetGravity(bool status)
    {
        if (rigid == null) return;

        rigid.useGravity = status;
    }
}
