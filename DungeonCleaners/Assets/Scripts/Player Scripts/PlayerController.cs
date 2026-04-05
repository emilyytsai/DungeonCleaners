using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movementInput;
    private Vector2 lastLookDirection = new Vector2(0, -1); // Default facing down

    // Hash parameters for performance (Best Practice)
    private readonly int animLookX = Animator.StringToHash("LookX");
    private readonly int animLookY = Animator.StringToHash("LookY");
    private readonly int animSpeed = Animator.StringToHash("Speed");
    private readonly int animHurt = Animator.StringToHash("Hurt");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 1. Gather Input (Replace with new Input System if using it)
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Normalize to prevent faster diagonal movement
        if (movementInput.sqrMagnitude > 1) 
            movementInput.Normalize();

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        // 2. Apply Physics in FixedUpdate
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimation()
    {
        // Only update look direction if we are actually moving
        if (movementInput != Vector2.zero)
        {
            lastLookDirection = movementInput;

            // Flip sprite based on X direction (if using a single side-facing sprite)
            if (movementInput.x < 0)
                spriteRenderer.flipX = true;
            else if (movementInput.x > 0)
                spriteRenderer.flipX = false;
        }

        // Pass data to Animator
        animator.SetFloat(animLookX, lastLookDirection.x);
        animator.SetFloat(animLookY, lastLookDirection.y);
        animator.SetFloat(animSpeed, movementInput.sqrMagnitude);
    }

    // Call this from a health/combat script
    public void TakeDamage()
    {
        animator.SetTrigger(animHurt);
    }
}