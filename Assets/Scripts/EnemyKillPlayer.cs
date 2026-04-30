using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        bool isStomping = false;
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isStomping = true;
                break;
            }
        }
        if (isStomping) return;

        Player player = col.gameObject.GetComponent<Player>();
        if (player == null) return;

        Vector2 knockDir = (col.transform.position - transform.position).normalized;
        knockDir = new Vector2(knockDir.x, 0.5f).normalized;
        player.TakeDamage(1, knockDir);
    }
}