using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    public State PlayerState
    {
        get
        {
            return playerState;
        }
        set
        {
            playerState = value;
        }
    }

    public GameObject Cursor
    {
        get
        {
            return arrow;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public float KnockbackStrength
    {
        get
        {
            return knockbackStrength;
        }
    }

    public float AttackMultiplier
    {
        get
        {
            return attackMultiplier;
        }
    }

    public Vector3 AimDirection
    {
        get 
        {
            return aimDirection;
        }
    }

    public event System.Action<int, int> OnHealthChanged;
    public event System.Action<int, int> OnExpChanged;

    public event System.Action<int> OnLevelChanged;

    public event System.Action OnPlayerDied;

    public event System.Action OnSummonBossEarly;

    [Header("Stats"), SerializeField] float speed = 1f;
    [SerializeField] float speedWhenAttacking = 1f;

    CameraShake camShake;
    [SerializeField] float camShakeAmountWhenDamaged = 1f;
    [SerializeField] float camShakeDurationWhenDamaged = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;
    Vector3 lastValidMoveDir;
    Vector3 aimDirection;

    Vector3 velocity;

    [SerializeField] AudioSource swooshSound;
    [SerializeField] AudioSource footstepSound;

    Animator anim;

    int level;
    int exp = 0;
    int expToNextLevel;

    [SerializeField] int baseAttack = 3;
    [SerializeField] int attackGainPerLevel = 3;
    int attack;
    float attackMultiplier = 1f;
    float attackStartedTime;

    [SerializeField] AnimationClip[] attackAnimations;
    
    [Range(1, 5), SerializeField] float attackTwoDamageMultiplier = 1.5f;
    [Range(1, 10), SerializeField] float attackThreeDamageMultiplier = 2f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float knockbackStrength = 1f;

    [SerializeField] int baseHealth = 5;
    [SerializeField] int healthGainPerLevel = 3;
    int health;
    int maxHealth;
    bool keepAttacking = false;

    float knockBackStarted;
    float knockBackDuration;
    Vector3 knockbackDir;

    [SerializeField] TitlescreenController titlescreen;

    bool isStopped = true;

    [SerializeField] float dashForce = 1f;

    PlayerInput input;

    Camera cam;

    LayerMask projectileLayer;

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject hitbox;
    [SerializeField] ParticleSystem footprints;
    ParticleSystem.MainModule footprintsMainModule;
    ParticleSystem.ShapeModule footprintsShapeModule;
    ParticleSystem.EmissionModule footprintsEmissionModule;
    ParticleSystem.MinMaxCurve standardRateOverDistance;

    [SerializeField] ParticleSystem dash;
    ParticleSystem.EmissionModule dashEmission;

    public GameObject projectile;

    public float yOffset;
    public float by;
    float layer;
    SpriteRenderer rend;
    Vector3 centerBottom;

    float bossSummonCount = 0f;

    public enum State
    {
        freeToMove,
        dashing,
        attacking,
        attackingTwo,
        attackingThree,
        knockedBack
    }
    State playerState = State.freeToMove;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        cam = Camera.main;
        anim = GetComponent<Animator>();

        titlescreen.OnGameStarted += OnGameStarted;

        attack = baseAttack;
        health = baseHealth;
        maxHealth = baseHealth;

        expToNextLevel = (int)(Mathf.Pow(level, 2) * 2f);

        footprintsMainModule = footprints.main;
        footprintsShapeModule = footprints.shape;
        footprintsEmissionModule = footprints.emission;

        dashEmission = dash.emission;

        rend = GetComponent<SpriteRenderer>();

        int layer = LayerMask.NameToLayer("PlayerProjectiles");
        projectileLayer = 1 << layer;

        camShake = Camera.main.GetComponent<CameraShake>();

    }

    private void Start()
    {
        if (OnExpChanged != null)
        {
            OnExpChanged(exp, expToNextLevel);
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged(health, maxHealth);
        }
        if (OnLevelChanged != null)
        {
            OnLevelChanged(level);
        }
    }

    private void Update()
    {
        if(isStopped) { return; }
        CalculateOrderInLayer();

        //unparent the particle system and it does work
        footprints.transform.position = transform.position + new Vector3(0, -0.8f);
        dash.transform.position = transform.position;
        
        if (exp >= expToNextLevel)
        {
            LevelUp();
        }

        if (playerState == State.freeToMove)
        {
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("AttackTwo");
            anim.ResetTrigger("AttackThree");
            footprintsEmissionModule.rateOverDistance = 1f;
            if (dash)
            {
                dashEmission.rateOverDistance = 0f;
            }
            GetInput();
            velocity = moveDirection * speed;
            if (input.UseItem && playerItem)
            {
                playerItem.Use();
            }
            if (input.UseSkill && playerSkill)
            {
                playerSkill.Use();
            }
            if (input.Attack && Time.realtimeSinceStartup > attackStartedTime + attackAnimations[0].length+ attackCooldown && !EventSystem.current.IsPointerOverGameObject())
            {
                keepAttacking = false;
                playerState = State.attacking;
                attackMultiplier = 1f;
                anim.SetTrigger("Attack");
                attackStartedTime = Time.realtimeSinceStartup;
            }
            if(input.SummonBoss && GameManager.Instance.IsStarted)
            {
                bossSummonCount += 0.01f;
            }
            else if(bossSummonCount > 0f)
            {
                bossSummonCount = 0f;
            }
            if(bossSummonCount >= 1f)
            {
                if(OnSummonBossEarly != null)
                {
                    OnSummonBossEarly();
                }
            }
        }
        else if (playerState == State.attacking)
        {
            GetInput();
            velocity = moveDirection * speedWhenAttacking;
            if (input.Attack)
            {
                keepAttacking = true;
            }
            if (Time.realtimeSinceStartup > attackStartedTime + attackAnimations[0].length && !keepAttacking)
            {
                playerState = State.freeToMove;
            }
            else if (Time.realtimeSinceStartup > attackStartedTime + attackAnimations[0].length && keepAttacking)
            {
                keepAttacking = false;
                playerState = State.attackingTwo;
                attackMultiplier = attackTwoDamageMultiplier;
                anim.SetTrigger("AttackTwo");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if (playerState == State.attackingTwo)
        {
            GetInput();
            velocity = moveDirection * speedWhenAttacking;
            if (input.Attack)
            {
                keepAttacking = true;
            }
            if (Time.realtimeSinceStartup > attackStartedTime + attackAnimations[1].length && !keepAttacking)
            {
                playerState = State.freeToMove;
            }
            else if (Time.realtimeSinceStartup > attackStartedTime + attackAnimations[1].length && keepAttacking)
            {
                playerState = State.attackingThree;
                attackMultiplier = attackThreeDamageMultiplier;
                anim.SetTrigger("AttackThree");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if (playerState == State.attackingThree)
        {
            GetInput();
            velocity = moveDirection * 0f;
            if (Time.realtimeSinceStartup > attackStartedTime + attackAnimations[2].length)
            {
                playerState = State.freeToMove;
            }
        }
        else if (playerState == State.dashing)
        {
            velocity = lastValidMoveDir.normalized * dashForce;
            footprintsEmissionModule.rateOverDistance = 0f;
            if (dash)
            {
                dashEmission.rateOverDistance = 2f;
            }
            // TODO Set the dash animation
        }
        else if (playerState == State.knockedBack)
        {
            if (Time.realtimeSinceStartup <= knockBackStarted + knockBackDuration)
            {
                velocity = knockbackDir * ((knockBackStarted + knockBackDuration) - Time.realtimeSinceStartup) * Time.deltaTime;
            }
            else
            {
                playerState = State.freeToMove;
            }
            StartCoroutine(FlashSprite(0.1f));
        }
        transform.position += velocity * Time.deltaTime;
        // Apply drag
        velocity = velocity * (1 - Time.deltaTime * 0.1f);
    }

    void OnGameStarted()
    {
        StartCoroutine(ActivatePlayer());
    }

    IEnumerator ActivatePlayer()
    {
        yield return new WaitForSeconds(2f);
        isStopped = false;
    }

    IEnumerator FlashSprite(float offtime)
    {
        rend.enabled = false;
        yield return new WaitForSeconds(offtime);
        rend.enabled = true;
    }

    private void CalculateOrderInLayer()
    {
        centerBottom = transform.TransformPoint(rend.sprite.bounds.min);

        layer = centerBottom.y + yOffset;

        rend.sortingOrder = -(int)(layer * 10);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + aimDirection);
    }

    public void PlayAttackSound()
    {
        if (swooshSound)
        {
            swooshSound.PlayOneShot(swooshSound.clip);
        }
    }

    public void GainExp(int expGain)
    {
        exp += expGain;
        if(OnExpChanged != null)
        {
            OnExpChanged(exp, expToNextLevel);
        }
    }

    void LevelUp()
    {
        attack += attackGainPerLevel;
        maxHealth += healthGainPerLevel;
        health = maxHealth;
        level++;
        exp = exp - expToNextLevel;
        expToNextLevel = (int)(Mathf.Pow(level, 2) * 2f);
        if(OnExpChanged != null)
        {
            OnExpChanged(exp, expToNextLevel);
        }
        if(OnHealthChanged != null)
        {
            OnHealthChanged(health, maxHealth);
        }
        if(OnLevelChanged != null)
        {
            OnLevelChanged(level);
        }
        // TODO call delegate to update level ui number
    }
    
    void GetInput()
    {
        moveDirection.x = input.Horizontal;
        if(moveDirection.x < 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(moveDirection.x > 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        moveDirection.y = input.Vertical;
        anim.SetFloat("Velocity", moveDirection.magnitude);
        footstepSound.volume = moveDirection.normalized.magnitude;
        if(moveDirection.magnitude < 0.1f && playerState == State.freeToMove)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveDirection.magnitude < 0.1f && playerState == State.attacking)
        {
            if(aimDirection.x < 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        // Create the angle for the movement vector
        float moveAngle = Vector3.Angle(Vector3.up, lastValidMoveDir);
        if (moveDirection.x < 0f)
        {
            moveAngle = -moveAngle;
        }
        if (dash)
        {
            dash.transform.up = -lastValidMoveDir;
        }
        if (footprints)
        {
            footprintsShapeModule.rotation = new Vector3(0f, moveAngle, 0f);
            footprintsMainModule.startRotation = 0.0175f * moveAngle;
        }
        // Only overwrite lastValidMoveDir if the player is not standing still. To always dash in a direction
        if(!HelperMethods.V3Equal(moveDirection, Vector3.zero, 0.01f))
        {
            lastValidMoveDir = moveDirection;
        }
        if(GameManager.Instance.IsControllerInput)
        {
            if(!HelperMethods.V3Equal(moveDirection, Vector3.zero, 0.1f))
            {
                aimDirection = moveDirection;
            }
        }
        else
        {
            Vector3 mousePosInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetAim = (mousePosInWorld - transform.position);
            aimDirection.x = targetAim.normalized.x;
            aimDirection.y = targetAim.normalized.y;
        }
        SetArrow();
        SetHitbox();
    }

    // Set the arrow position and rotation
    void SetArrow()
    {
        arrow.transform.position = (transform.position + aimDirection.normalized);
        arrow.transform.position = new Vector3(arrow.transform.position.x, arrow.transform.position.y - 0.5f);
        arrow.transform.rotation = Quaternion.FromToRotation(arrow.transform.up, aimDirection) * arrow.transform.rotation;
    }

    // Set the hitbox position and rotation
    void SetHitbox()
    {
        hitbox.transform.position = transform.position + aimDirection.normalized;
        hitbox.transform.position = new Vector3(hitbox.transform.position.x, hitbox.transform.position.y - 0.5f);
        hitbox.transform.rotation = Quaternion.FromToRotation(hitbox.transform.up, aimDirection) * hitbox.transform.rotation;
    }

    // Subtracts damage from the player health and knocks him back
    public void TakeDamage(int damage, Vector3 knockback, float time, float duration)
    {
        // Player only takes damage, if he isnt already knocked back
        if(playerState != State.knockedBack)
        {
            camShake.shakeAmount = camShakeAmountWhenDamaged;
            camShake.shakeDuration = camShakeDurationWhenDamaged;
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            knockbackDir = knockback;
            knockBackStarted = time;
            knockBackDuration = duration;
            playerState = State.knockedBack;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(health, maxHealth);
            }
        }
    }

    void Die()
    {
        camShake.shakeDuration = 0f;
        isStopped = true;
        footstepSound.Stop();
        if(OnPlayerDied != null)
        {
            OnPlayerDied();
        }
    }

    // Collects Items or Skills if player walks over them if nothing is equipped before
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BaseItem>() && !playerItem)
        {
            collision.GetComponent<BaseItem>().Equipped = true;
            playerItem = collision.GetComponent<BaseItem>();
        }
        else if(collision.GetComponent<BaseSkill>() && !playerSkill)
        {
            collision.GetComponent<BaseSkill>().Equipped = true;
            playerSkill = collision.GetComponent<BaseSkill>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<BaseItem>())
        {
            if (input.SwitchPowerup && playerItem)
            {
                playerItem.Equipped = false;
                collision.GetComponent<BaseItem>().Equipped = true;
                playerItem = collision.GetComponent<BaseItem>();
            }
        }
        else if (collision.GetComponent<BaseSkill>())
        {
            if(input.SwitchPowerup && playerSkill)
            {
                playerSkill.Equipped = false;
                collision.GetComponent<BaseSkill>().Equipped = true;
                playerSkill = collision.GetComponent<BaseSkill>();
            }
        }
    }
}
