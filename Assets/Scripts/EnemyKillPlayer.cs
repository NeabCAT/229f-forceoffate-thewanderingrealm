using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = col.gameObject.GetComponent<Rigidbody2D>();

        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f) return;
        }

        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
            player.TakeDamage(player.GetCurrentHealth());
    }
}