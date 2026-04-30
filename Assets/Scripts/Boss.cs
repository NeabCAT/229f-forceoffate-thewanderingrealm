using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform rightPoint;
    public GameObject bulletPrefab;

    public int hp = 6;
    public int maxHp = 6;

    public Transform[] spawnPoints;

    private int facingDir = 1;

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

        Transform target = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = target.position;

        facingDir = -facingDir;
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * facingDir,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
