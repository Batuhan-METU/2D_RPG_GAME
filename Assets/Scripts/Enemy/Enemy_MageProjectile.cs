using UnityEngine;

public class Enemy_MageProjectile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private LayerMask whatCanCollideWith;

    public void SetupProjectile(Transform target, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        anim.enabled = false;
        this.combat = combat;

        Vector2 velocity = CalculateBallisticVelocity(transform.position, target.position);
        rb.linearVelocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatCanCollideWith) != 0)
        {
            combat.PerformAttackOnTarget(collision.transform);

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.enabled = true;
            col.enabled = false;
            Destroy(gameObject, 2);
        }
    }


    /// <summary>
    /// Calculates the initial velocity needed to launch a projectile from `start` to `end`
    /// with a ballistic arc that reaches `arcHeight` at the peak.
    /// Assumes gravity is enabled on the Rigidbody2D.
    /// </summary>

    private Vector2 CalculateBallisticVelocity(Vector2 start, Vector2 end)
    {
        // Get effective gravity based on global gravity and this Rigidbody's gravity scale
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        // Calculate vertical and horizontal displacemnt 
        float displacementY = end.y - start.y;
        float displacementX = end.x - start.x;

        float peakHieght = Mathf.Max(arcHeight , end.y - start.y + .1f); // Ensure arc is always above

        // Time to reaach the top of the arc
        float timeToApex = Mathf.Sqrt(2 * peakHieght / gravity);

        // Time to fall from the top of arc to the target
        float timeFromApex = Mathf.Sqrt(2 * (peakHieght - displacementY) / gravity);

        // Total flight time = up + down
        float totalTime = timeToApex + timeFromApex;

        // Initial vertical velocity to reach the arc height
        float velocityY = Mathf.Sqrt(2 * gravity * peakHieght);

        // Initial horizontal velocity to cover distance in total flight time
        float velocityX = displacementX / totalTime;

        // Return combined velocity
        return new Vector2(velocityX, velocityY);
    }
}
