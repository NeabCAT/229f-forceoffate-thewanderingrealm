using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float climbSpeed = 3f;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;
    public float invincibleDuration = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public float coyoteTime = 0.15f;

    [Header("Knockback")]
    public float knockbackForce = 7f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private Coroutine knockbackCoroutine;

    [Header("Sound - Footstep")]
    public AudioClip[] footstepClips;
    public float footstepInterval = 0.35f;

    [Header("Sound - Jump / Land")]
    public AudioClip jumpClip;
    public AudioClip landClip;

    [Header("Sound - Climb")]
    public AudioClip[] climbStepClips;
    public float climbStepInterval = 0.4f;

    [Header("Sound - Hurt / Death")]
    public AudioClip hurtClip;
    public AudioClip deathClip;

    [Header("Sound - Audio Sources")]
    public AudioSource sfxSource;
    public AudioMixerGroup sfxMixerGroup;

    [Header("Sound - Volume")]
    [Range(0f, 1f)] public float footstepVolume = 0.5f;
    [Range(0f, 1f)] public float climbVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private float footstepTimer = 0f;
    private float climbStepTimer = 0f;
    private bool wasGrounded = false;

    private float coyoteTimeCounter;
    private Rigidbody2D rb;
    public bool isGrounded;
    private float moveInput;
    private Vector3 originalScale;

    public bool isGrappling = false;
    public bool canClimb = false;
    public bool isClimbing = false;
    private Animator animator;
    private bool isMovementLocked = false;

    [HideInInspector] public bool climbCanGoUp = false;
    [HideInInspector] public bool climbCanGoDown = false;

    private bool isOnMovingPlatform = false;
    private MovingPlatform currentPlatform = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        if (sfxMixerGroup != null)
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    void Update()
    {
        animator.SetBool("isBlocking", isMovementLocked);
        if (isDead || isGrappling || isMovementLocked) return;

        bool groundedThisFrame = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (groundedThisFrame)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        isGrounded = coyoteTimeCounter > 0f;

        if (groundedThisFrame && !wasGrounded
            && rb.linearVelocity.y < -2f
            && !isOnMovingPlatform)
            PlaySound(landClip);
        wasGrounded = groundedThisFrame;

        moveInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("isWalking", moveInput != 0 && groundedThisFrame);
        animator.SetBool("isJumping", !isGrounded && !isClimbing && rb.linearVelocity.y > 0.1f);
        animator.SetBool("isFalling", !groundedThisFrame && !isClimbing
                                      && rb.linearVelocity.y < -0.5f
                                      && !isOnMovingPlatform);
        animator.SetBool("isClimbing", isClimbing);

        HandleFootstepSound(groundedThisFrame);

        HandleClimbSound();

        if (canClimb && Input.GetKeyDown(KeyCode.W))
        {
            isClimbing = true;
            Ladder ladder = FindObjectOfType<Ladder>();
            if (ladder != null)
                ladder.DisableGroundCollision(GetComponent<Collider2D>());
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            PlaySound(jumpClip);
        }

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(moveInput) * Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }

    void FixedUpdate()
    {
        if (isDead || isGrappling || isMovementLocked || isKnockedBack) return;

        float platformVelX = currentPlatform != null ? currentPlatform.PlatformVelocity.x : 0f;

        if (isClimbing)
        {
            float climbInput = 0f;
            if (Input.GetKey(KeyCode.W)) climbInput = 1f;
            if (Input.GetKey(KeyCode.S)) climbInput = -1f;
            rb.linearVelocity = new Vector2(0f, climbInput * climbSpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed + platformVelX, rb.linearVelocity.y);
        }
    }

    private void PlaySound(AudioClip clip, float volume = -1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume < 0 ? sfxVolume : volume);
    }

    private void PlayRandomSound(AudioClip[] clips, float volume)
    {
        if (clips == null || clips.Length == 0 || sfxSource == null) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    private void HandleFootstepSound(bool groundedThisFrame)
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.01f && groundedThisFrame && !isClimbing;
        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayRandomSound(footstepClips, footstepVolume);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void HandleClimbSound()
    {
        if (!isClimbing) { climbStepTimer = 0f; return; }

        float climbInput = 0f;
        if (Input.GetKey(KeyCode.W)) climbInput = 1f;
        if (Input.GetKey(KeyCode.S)) climbInput = -1f;

        if (Mathf.Abs(climbInput) > 0.01f)
        {
            climbStepTimer -= Time.deltaTime;
            if (climbStepTimer <= 0f)
            {
                PlayRandomSound(climbStepClips, climbVolume);
                climbStepTimer = climbStepInterval;
            }
        }
        else
        {
            climbStepTimer = 0f;
        }
    }

    public void TakeDamage(int amount, Vector2 knockbackDir = default)
    {
        if (isDead) return;

        if (knockbackDir != Vector2.zero)
        {
            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackDir));
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
        else
            PlaySound(hurtClip);  
    }

    IEnumerator KnockbackCoroutine(Vector2 dir)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
        knockbackCoroutine = null;
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        PlaySound(deathClip);  
        Debug.Log("Player died!");

        PlayerRespawn respawn = GetComponent<PlayerRespawn>();
        if (respawn != null)
            respawn.Respawn();

        CameraFollow cam = FindFirstObjectByType<CameraFollow>();
        if (cam != null) cam.SnapToTarget();

        currentHealth = maxHealth;
        isDead = false;
    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    void OnCollisionEnter2D(Collision2D col) { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SpawnPoint>(out SpawnPoint sp))
            SpawnManager.Instance.TryActivate(sp);

        if (other.CompareTag("DeathZone"))
            TakeDamage(currentHealth);
    }

    public void SetMovementLocked(bool locked)
    {
        isMovementLocked = locked;
        if (locked)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void SetOnMovingPlatform(bool value, MovingPlatform platform)
    {
        isOnMovingPlatform = value;
        currentPlatform = platform;
    }

    public int GetCurrentHealth() => currentHealth;
    public bool IsDead() => isDead;

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void SetHealth(int hp)
    {
        currentHealth = Mathf.Clamp(hp, 0, maxHealth);
    }
}