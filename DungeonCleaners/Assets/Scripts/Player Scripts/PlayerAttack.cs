using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference attackAction;

    [Header("Settings")]
    public float attackRange = 1.5f;
    public int damageAmount = 10;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private Animator _anim;
    private PlayerController _controller;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    private void OnEnable() { if (attackAction != null) attackAction.action.Enable(); }
    private void OnDisable() { if (attackAction != null) attackAction.action.Disable(); }

    void Update()
    {
        // Null-safe check for Input Action and Keyboard
        bool inputActionPressed = (attackAction != null && attackAction.action != null && attackAction.action.WasPressedThisFrame());
        bool spaceKeyPressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);

        if ((inputActionPressed || spaceKeyPressed) && Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void PerformAttack()
    {
        // Console Log: Player Input Received
        Debug.Log("<color=yellow>Player: Swung the mop (Space/Attack pressed)!</color>");

        if (_anim != null) _anim.SetTrigger("Attack");

        // Determine direction from PlayerController
        Vector2 direction = Vector2.down;
        if (_controller != null) direction = _controller.GetLastLookDirection();

        Vector2 attackPoint = (Vector2)transform.position + direction * 0.8f;

        // Detect enemies on the specific Layer
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayer);

        if (hitEnemies.Length == 0)
        {
            Debug.Log("<color=white>Player: Swung but hit nothing.</color>");
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            BossHealth boss = enemy.GetComponent<BossHealth>();
            if (boss != null)
            {
                Debug.Log("<color=orange>Player: Hit detected on " + enemy.name + "!</color>");
                boss.TakeDamage(damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 direction = Vector2.down;
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) direction = pc.GetLastLookDirection();
        Gizmos.DrawWireSphere((Vector2)transform.position + direction * 0.8f, attackRange);
    }
}