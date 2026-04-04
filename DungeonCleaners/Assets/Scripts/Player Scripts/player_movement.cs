using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Automatically grab the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get raw input for snappy, immediate response
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the vector to ensure the player doesn't move faster diagonally
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Apply the movement to the Rigidbody2D
        // Time.fixedDeltaTime ensures movement is frame-rate independent
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}