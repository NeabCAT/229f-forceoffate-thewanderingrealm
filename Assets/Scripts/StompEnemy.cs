using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    [Header("Stomp Settings")]
    public float stompBounceForce = 8f;
    public float stompThreshold = 0.5f; 

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = col.gameObject.GetComponent<Rigidbody2D>();

        foreach (ContactPoint2D contact in col.contacts)
        {
            bool isAbove = contact.normal.y > 0.5f; 
            bool isFalling = playerRb.linearVelocity.y <= 0;

            if (isAbove && isFalling)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);
                Destroy(gameObject);
                return;
            }
        }
    }
}