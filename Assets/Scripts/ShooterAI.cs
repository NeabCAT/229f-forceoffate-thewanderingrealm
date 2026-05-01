using UnityEngine;

public class ShooterAI : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float shootRange = 5f;

    [Header("Detection")]
    public float detectRange = 7f;

    [Header("Facing")]
    public float facingDirection = 1f;
    private float fireTimer = 0f;
    private float dir;
    private EnemyContact enemyContact;
    private Animator animator;
    private bool playerInRange = false;

    [Header("Sound")]
    public AudioClip shootClip;
    public AudioSource sfxSource;

    void Start()
    {
        dir = facingDirection >= 0 ? 1f : -1f;
        enemyContact = GetComponent<EnemyContact>();
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (enemyContact != null && enemyContact.isDead)
        {
            if (animator != null)
            {
                animator.SetBool("isShooting", false);
                animator.SetBool("isDead", true);
            }
            return;
        }

        // ✅ เช็คระยะ Player
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            playerInRange = Vector2.Distance(transform.position, p.transform.position) <= detectRange;

        if (!playerInRange)
        {
            fireTimer = 0f;
            return;
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate)
        {
            if (animator != null)
                animator.SetBool("isShooting", true);
            fireTimer = 0f;
        }
    }

    public void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;
        if (enemyContact != null && enemyContact.isDead) return;
        if (!playerInRange) return;  // ✅ ถ้า Player ออกนอกระยะ ไม่ยิง
        if (animator != null)
            animator.SetBool("isShooting", false);
        if (shootClip != null && sfxSource != null)
            sfxSource.PlayOneShot(shootClip);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir > 0 ? 1 : -1, shootRange);
    }

    void OnDrawGizmos()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(firePoint.position, Vector2.right * (facingDirection >= 0 ? 1f : -1f) * shootRange);
        Gizmos.color = Color.yellow;  // ✅ แสดง detection range
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}