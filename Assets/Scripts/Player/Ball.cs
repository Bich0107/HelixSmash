using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] Vector3 basePos;
    [SerializeField] Vector3 baseScale;
    [SerializeField] List<SpherePart> parts;
    [SerializeField] ExplodeAnimation explodeAnimation;
    [SerializeField] CounterDisplayer displayer; // display number of brick destroyed
    [SerializeField] ParticleSystem fireVFX;
    [SerializeField] AudioClip collideSound;
    [SerializeField] AudioClip breakSound;
    [Header("Movement settings")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float minXScale;
    [SerializeField] float minYScale;
    [SerializeField] float scaleChangeRate;
    [Space]
    [SerializeField] float bounceHeight;
    [SerializeField] float bounceTime;
    [SerializeField] float bounceDistance;
    bool isBouncing;
    [Header("Attack settings")]
    [SerializeField] float dropSpeed;
    [SerializeField] float baseInvincibleTime = 3f;
    [SerializeField] float invincibleTime;
    [SerializeField] float invincibleTimer;
    [SerializeField] float timeIncreasePerDestroyedBrick = 0.001f;
    public float InvincibleTime => invincibleTime;
    public float InvincibleTimer => invincibleTimer;
    [Space]
    [Tooltip("Continuously destroy brickAmount brick to be invincible for invincibleTime")]
    [SerializeField] int brickAmount;
    [SerializeField] int brickCounter;
    [SerializeField] int continuousCounter;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isInvincible = false;

    public int BrickAmount => brickAmount;
    public int BrickCounter => brickCounter;
    public int ContinuousCounter => continuousCounter;

    // flags
    bool isFinish;
    bool isDead = false;
    public bool IsFinish => isFinish;
    public bool IsAttacking => isAttacking;
    public bool IsInvincible => isInvincible;
    public bool IsDead => isDead;

    void Awake()
    {
        basePos = transform.position;
        baseScale = transform.localScale;
    }

    void Start()
    {
        explodeAnimation = GetComponent<ExplodeAnimation>();
        rigid = GetComponent<Rigidbody>();
        displayer = FindObjectOfType<CounterDisplayer>();

        // reset display value
        displayer.Display(0);
    }

    void Update()
    {
        if (isDead || isFinish) return;

        ProcessMouseClick();
        ScaleAnimation();
    }

    // scale game object base on move direction
    void ScaleAnimation()
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;

        if (isBouncing)
        {  // when moving up, reduce increase y scale while increase x scale
            x = Mathf.Max(minXScale, x - Time.deltaTime * scaleChangeRate);
            y = Mathf.Min(baseScale.y, y + Time.deltaTime * scaleChangeRate);
            transform.localScale = new Vector3(x, y, transform.localScale.z);
        }
        else
        {   // when moving up, reduce increase x scale while increase y scale
            x = Mathf.Min(baseScale.y, x + Time.deltaTime * scaleChangeRate);
            y = Mathf.Max(minYScale, y - Time.deltaTime * scaleChangeRate);
            transform.localScale = new Vector3(x, y, transform.localScale.z);
        }
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

    // call when ball destroy a brick at its special part
    public void IncreaseInvincibleTime()
    {
        invincibleTime += timeIncreasePerDestroyedBrick;
    }

    public void IncreaseCounter()
    {
        brickCounter++; // total destroy counter
        continuousCounter++; // continously destroy counter
        
        displayer.Display(brickCounter);

        // if continouse counter reach the brickAmount and is not invincible, turn invincible
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

        // count down invincible time
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

        AudioManager.Instance.PlaySound(collideSound);
        isBouncing = true;
        SetGravity(false);
        bounceHeight = transform.position.y; // set the base height for this bounce
        StartCoroutine(CR_BounceUp());
    }

    IEnumerator CR_BounceUp()
    {
        float targetHeight = bounceHeight + bounceDistance; // calculate the max height base on start height

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

        // ensure ball is at the right height
        newPosition.y = targetHeight;
        transform.position = newPosition;

        // reset velocity and turn on gravity
        rigid.velocity = Vector3.zero;
        SetGravity(true);

        isBouncing = false;
    }

    public void Die()
    {
        isDead = true;
        Stop();

        AudioManager.Instance.PlaySound(breakSound);
        explodeAnimation.Play();
    }

    // call when collide with finish line
    public void Finish()
    {
        isFinish = true;
        transform.localScale = baseScale;
        Stop();
    }

    // stop all physics action and set gravity status
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

    public void ResetTransform()
    {
        transform.position = basePos;
        transform.localScale = baseScale;
        foreach (SpherePart part in parts)
        {
            part.Reset();
        }

        rigid.useGravity = true;
        isFinish = false;
    }

    public void Reset()
    {
        isDead = false;

        ResetTransform();
        brickCounter = 0;
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
