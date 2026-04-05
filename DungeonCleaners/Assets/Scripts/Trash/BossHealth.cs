using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public float damageCooldown = 0.4f;

    private int currentHealth;
    private float lastHurtTime;
    private Animator animator;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // Initial Console Log
        Debug.Log($"<color=cyan>Boss Spawned: {gameObject.name} with {maxHealth} HP.</color>");
    }

    public void TakeDamage(int damage)
    {
        // Prevent taking damage too fast (I-Frames)
        if (Time.time < lastHurtTime + damageCooldown) return;

        lastHurtTime = Time.time;
        currentHealth -= damage;

        // Console Log: Health Change
        Debug.Log($"<color=red>BOSS HP: {currentHealth} / {maxHealth} (-{damage} damage)</color>");

        if (animator != null)
        {
            // Ensure this matches your Animator parameter exactly
            animator.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("<color=green>Boss Defeated: Initiating cleanup!</color>");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Destroy the boss after the death animation (0.5s delay)
        Destroy(gameObject, 0.5f);
    }
}