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
        if (playerTransform == null || isStunned)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Updated flip logic for left-facing sprites
            FlipSprite(direction.x);
            
            anim.SetFloat("LookX", direction.x);
            anim.SetFloat("LookY", direction.y);
            anim.SetFloat("Speed", 1f); 
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    private void FlipSprite(float xDirection)
    {
        // INVERTED LOGIC: 
        // If moving right, use negative scale. If moving left, use positive scale.
        if (xDirection > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xDirection < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        
        yield return new WaitForSeconds(hitStunDuration);
        isStunned = false;
    }
}