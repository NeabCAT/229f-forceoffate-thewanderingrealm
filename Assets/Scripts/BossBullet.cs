using System.Collections;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 1;
    private Vector2 velocity;
    private bool hasBounced = false;

    public void InitBull(Vector2 dir)
    {
        velocity = dir.normalized * speed;
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Ground")) return;

        if (hit.CompareTag("Shield"))
        {
            velocity = -velocity;
            hasBounced = true;
            return;
        }

        if (hit.CompareTag("Enemy") && hasBounced)
        {
            Boss boss = hit.GetComponent<Boss>();
            if (boss != null)
                boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (hit.CompareTag("Player"))
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
                player.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
