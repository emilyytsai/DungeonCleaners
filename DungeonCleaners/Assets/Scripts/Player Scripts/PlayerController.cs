using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Input Settings")]
    [Tooltip("Drag your Move InputAction here from the Input Action Asset")]
    public InputActionReference moveAction; 

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movementInput;
    private Vector2 lastLookDirection = new Vector2(0, -1); // Default facing down

    // Hash parameters for performance
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

    // You MUST enable and disable Input Actions when using references
    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    private void Update()
    {
        // 1. Gather Input using the New System
        if (moveAction != null)
        {
            // Reads the Vector2 directly from your mapped WASD/Gamepad bindings
            movementInput = moveAction.action.ReadValue<Vector2>();
        }

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
        if (movementInput != Vector2.zero)
        {
            lastLookDirection = movementInput;

            if (movementInput.x < 0)
                spriteRenderer.flipX = true;
            else if (movementInput.x > 0)
                spriteRenderer.flipX = false;
        }

        animator.SetFloat(animLookX, lastLookDirection.x);
        animator.SetFloat(animLookY, lastLookDirection.y);
        animator.SetFloat(animSpeed, movementInput.sqrMagnitude);
    }

    public void TakeDamage()
    {
        animator.SetTrigger(animHurt);
    }
}