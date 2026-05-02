using UnityEngine;

public class Slime_Bite : MonoBehaviour
{
    [Header("Detection")]
    public float detectRange = 3f;

    [Header("Jump Settings")]
    public float jumpCooldown = 2f;
    public float t = 1f;
    public float heightOffset = 3f;

    private Rigidbody2D rb;
    private EnemyContact enemyContact;
    private Transform player;
    private bool isGrounded = false;
    private float cooldownTimer = 0f;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyContact = GetComponent<EnemyContact>();
        originalScale = transform.localScale;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (enemyContact != null && enemyContact.isDead) return;
        if (player == null) return;

        cooldownTimer -= Time.deltaTime;

        float facingDir = Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = new Vector3(
            Mathf.Abs(originalScale.x) * -facingDir,
            originalScale.y,
            originalScale.z
        );

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectRange && isGrounded && cooldownTimer <= 0f)
        {
            JumpToPlayer();
            cooldownTimer = jumpCooldown;
        }
    }

    void JumpToPlayer()
    {
        Vector2 direction = (player.position - transform.position);
        direction.y += heightOffset;

        Vector2 velocity = new Vector2(
            direction.x / t,
            (direction.y / t) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * t
        );

        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = velocity;
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;
            }
        }
    }
}
