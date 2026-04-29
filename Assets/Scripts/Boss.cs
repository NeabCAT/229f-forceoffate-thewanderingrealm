using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform rightPoint;
    public GameObject bulletPrefab;

    public int hp = 6;

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
                b.InitBull(Vector2.right);
            yield return new WaitForSeconds(2f);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        Destroy(gameObject);
    }
}
