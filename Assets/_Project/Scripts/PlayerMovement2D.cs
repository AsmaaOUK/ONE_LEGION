using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    // Components
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // COMBAT SYSTEM
    public float attackDamage = 25f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public float lastAttackTime = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Get horizontal input
        float moveInput = Input.GetAxis("Horizontal");
        
        // Move the player
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        // Jump when Space is pressed and player is on ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
        // Trigger attack animation
        if (Input.GetKeyDown(KeyCode.F) && animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Update walk animation
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
        
        // Flip sprite based on direction
        if (spriteRenderer != null && moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }
    }
    
    // Check if player is touching ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}