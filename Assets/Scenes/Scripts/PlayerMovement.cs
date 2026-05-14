using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float wallSlideSpeed = 1.5f;
    public float wallJumpHorizontalForce = 6f;
    public float wallJumpVerticalForce = 7f;
    public float wallCheckDistance = 0.6f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isTouchingWall;
    private int wallDirection;
    private int jumpsRemaining;
    private int currentJump;
    private const int maxJumps = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpsRemaining = maxJumps;
        currentJump = 0;
    }

    void Update()
    {
        // Left & Right movement
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Wall check and wall slide
        isTouchingWall = CheckWallContact(out wallDirection);
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }

        // Set animator parameters
        animator.SetBool("IsOnGround", isGrounded);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetBool("IsJumping", currentJump == 1);
        animator.SetBool("IsDoubleJumping", currentJump == 2);
        animator.SetBool("Isrunning", move != 0);

        // Flip sprite based on direction
        if (move < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (move > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTouchingWall && !isGrounded)
            {
                rb.velocity = new Vector2(wallJumpHorizontalForce * wallDirection, wallJumpVerticalForce);
                jumpsRemaining = maxJumps - 1;
                currentJump = 1;
                isGrounded = false;
            }
            else if (jumpsRemaining > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsRemaining--;
                currentJump++;
            }
        }
    }

    private bool CheckWallContact(out int direction)
    {
        direction = 0;
        Vector2 position = transform.position;

        RaycastHit2D rightHit = Physics2D.Raycast(position, Vector2.right, wallCheckDistance);
        if (rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            direction = -1;
            return true;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(position, Vector2.left, wallCheckDistance);
        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground"))
        {
            direction = 1;
            return true;
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpsRemaining = maxJumps;
            currentJump = 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpsRemaining = maxJumps;
            currentJump = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
