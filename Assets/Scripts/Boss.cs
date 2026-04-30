using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform rightPoint;
    public GameObject bulletPrefab;
    public int hp = 6;
    public int maxHp = 6;
    public Transform[] spawnPoints;
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่

    private int facingDir = 1;
    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
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
            Destroy(gameObject);
            return;
        }
        MoveToRandomPosition();
    }

    void MoveToRandomPosition()
    {
        if (spawnPoints.Length == 0) return;
        if (isMoving) return; // ถ้ากำลังเดินอยู่ ไม่ต้องสั่งใหม่

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

        transform.position = targetPos; // snap ให้ตรงเป๊ะตอนถึง
        isMoving = false;
    }
}