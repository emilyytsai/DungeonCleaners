using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float stopDistance = 1.2f;

    [Header("Combat Settings")]
    [SerializeField] private float hitStunDuration = 0.5f;

    // References
    private Transform playerTransform;
    private Animator anim;
    private Rigidbody rb;
    private bool isStunned = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Find the player by tag at the start
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Don't move if player is missing or if the enemy is currently stunned (hit)
        if (playerTransform == null || isStunned) return;

        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stopDistance)
        {
            // Calculate direction
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            
            // Look at player (Y-axis only to prevent tilting)
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            // Move using Translate for simplicity, multiplied by Time.deltaTime for frame-rate independence
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    // Public method to be called by player weapons/projectiles
    public void TakeDamage(int damageAmount)
    {
        if (isStunned) return; // Prevent "hit-spamming" during stun if desired

        Debug.Log($"Enemy hit for {damageAmount} damage!");

        // Trigger the animation
        anim.SetTrigger("Hit");

        // Start the reaction (pause movement)
        StartCoroutine(HitStunRoutine());
    }

    private System.Collections.IEnumerator HitStunRoutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(hitStunDuration);
        isStunned = false;
    }
}