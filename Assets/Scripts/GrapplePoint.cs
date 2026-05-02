using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [Header("General")]
    public float grappleRange = 5f;

    [Header("Smooth Pull (Tap Mode)")]
    public float targetSpeed = 12f;
    public float steerForce = 5f;
    public float slowRadius = 2f;

    [Header("Rope Physics")]
    public float ropeStiffness = 60f;
    public float swingForce = 10f;

    [Header("Auto Release (Tap Mode)")]
    public float stopDistance = 0.5f;
    public float maxGrappleTime = 2f;
    public float minVelocityToKeep = 0.5f;

    [Header("Mode")]
    public bool holdToGrapple = true;

    public LineRenderer lineRenderer;

    private Rigidbody2D playerRb;
    private Transform player;
    private Player playerScript;       

    private bool isGrappling = false;
    private float ropeLength;
    private float grappleTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<Player>();  

        playerRb.linearDamping = 1f;

        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (holdToGrapple)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!isGrappling && dist <= grappleRange)
                    StartGrapple();
            }

            if (Input.GetKeyUp(KeyCode.E))
                StopGrapple();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isGrappling)
                {
                    StopGrapple();
                    return;
                }

                if (dist <= grappleRange)
                    StartGrapple();
            }
        }

        if (isGrappling && lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.position);
        }
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

        grappleTimer += Time.fixedDeltaTime;

        Vector2 anchor = transform.position;
        Vector2 pos = playerRb.position;
        float dist = Vector2.Distance(anchor, pos);

        if (!holdToGrapple && dist <= stopDistance) { StopGrapple(); return; }
        if (!holdToGrapple && grappleTimer > maxGrappleTime) { StopGrapple(); return; }
        if (!holdToGrapple && playerRb.linearVelocity.magnitude < minVelocityToKeep) { StopGrapple(); return; }

        Vector2 toPlayer = pos - anchor;
        if (toPlayer.magnitude == 0) return;

        Vector2 dir = toPlayer.normalized;

        float outwardVel = Vector2.Dot(playerRb.linearVelocity, dir);
        if (dist > ropeLength && outwardVel > 0)
            playerRb.linearVelocity -= dir * outwardVel;

        float stretch = dist - ropeLength;
        if (stretch > 0)
            playerRb.AddForce(-dir * stretch * ropeStiffness);

        if (holdToGrapple)
        {
            Vector2 tangent = new Vector2(-dir.y, dir.x);
            float input = Input.GetAxisRaw("Horizontal");
            playerRb.AddForce(tangent * input * swingForce);
        }

        if (!holdToGrapple)
        {
            float speedMultiplier = Mathf.Clamp01(dist / slowRadius);
            Vector2 desiredVel = -dir * targetSpeed * speedMultiplier;
            Vector2 steering = desiredVel - playerRb.linearVelocity;
            playerRb.AddForce(steering * steerForce);
        }
    }

    void StartGrapple()
    {
        isGrappling = true;
        grappleTimer = 0f;

        float dist = Vector2.Distance(transform.position, player.position);

        ropeLength = holdToGrapple ? dist : dist * 0.7f;

        if (lineRenderer != null)
            lineRenderer.enabled = true;

        if (!holdToGrapple)
        {
            Vector2 dir = ((Vector2)transform.position - playerRb.position).normalized;
            playerRb.AddForce(dir * 5f, ForceMode2D.Impulse);
        }

        if (playerScript != null)
        {
            playerScript.isGrappling = true;
            playerScript.OnGrappleAttach();  
        }
    }

    void StopGrapple()
    {
        if (!isGrappling) return;

        isGrappling = false;

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        if (playerScript != null)
            playerScript.isGrappling = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GetCenter(), grappleRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetCenter(), stopDistance);

        DrawGrid(GetCenter(), grappleRange);
    }

    void DrawGrid(Vector2 center, float size)
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
        float step = 1f;

        for (float x = -size; x <= size; x += step)
            Gizmos.DrawLine(new Vector2(center.x + x, center.y - size),
                            new Vector2(center.x + x, center.y + size));

        for (float y = -size; y <= size; y += step)
            Gizmos.DrawLine(new Vector2(center.x - size, center.y + y),
                            new Vector2(center.x + size, center.y + y));
    }

    Vector2 GetCenter()
    {
        Collider2D col = GetComponent<Collider2D>();
        return col != null ? (Vector2)col.bounds.center : (Vector2)transform.position;
    }
}