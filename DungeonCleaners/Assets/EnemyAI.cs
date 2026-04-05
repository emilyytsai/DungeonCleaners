using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float stopDistance = 0.5f;

    [Header("Combat Settings")]
    [SerializeField] private float hitStunDuration = 0.4f;

    private Transform playerTransform;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isStunned = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null || isStunned) return;

        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        // Calculate 2D distance
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > stopDistance)
        {
            // Get direction on X and Y axes
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Move the transform
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Flip the sprite based on direction
            FlipSprite(direction.x);
            
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    private void FlipSprite(float xDirection)
    {
        // If moving right (pos x) and scale is negative, or moving left (neg x) and scale is positive
        if (xDirection > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xDirection < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isStunned) return;
        
        anim.SetTrigger("Hit");
        StartCoroutine(HitStunRoutine());
    }

    private System.Collections.IEnumerator HitStunRoutine()
    {
        isStunned = true;
        // Optional: Zero out velocity if using non-kinematic Rigidbody2D
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        
        yield return new WaitForSeconds(hitStunDuration);
        isStunned = false;
    }
}