using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip shootClip;
    public AudioClip deathClip;
    public AudioSource sfxSource;

    public Transform rightPoint;
    public GameObject bulletPrefab;
    public int hp = 6;
    public int maxHp = 6;
    public Transform[] spawnPoints;
    public float moveSpeed = 5f;

    private int facingDir = 1;
    private bool isMoving = false;

    void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (shootClip != null && sfxSource != null)
                sfxSource.PlayOneShot(shootClip);

            GameObject bullet = Instantiate(bulletPrefab, rightPoint.position, Quaternion.identity);
            BossBullet b = bullet.GetComponent<BossBullet>();
            if (b != null)
                b.InitBull(facingDir == 1 ? Vector2.right : Vector2.left);
            yield return new WaitForSeconds(2f);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            if (deathClip != null && sfxSource != null)
                sfxSource.PlayOneShot(deathClip);

            Destroy(gameObject);
            return;
        }
        MoveToRandomPosition();
    }


    void MoveToRandomPosition()
    {
        if (spawnPoints.Length == 0) return;
        if (isMoving) return;

        Transform target = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // พลิก facing ก่อนเดิน
        facingDir = -facingDir;
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * facingDir,
            transform.localScale.y,
            transform.localScale.z
        );

        StartCoroutine(SmoothMove(target.position));
    }

    IEnumerator SmoothMove(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}