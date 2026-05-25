using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float wallSlideSpeed = 1.5f;
    public float wallJumpHorizontalForce = 6f;
    public float wallJumpVerticalForce = 7f;
    public float wallCheckDistance = 0.6f;
	public LayerMask groundLayer;

	private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isTouchingWall;
    private int wallDirection;
    private int jumpsRemaining;
    private int currentJump;
    private const int maxJumps = 2;

    private float moveInput;
    private bool jumpPressed;

	public float acceleration = 6f;
	public float deceleration = 8f;
	public float airAcceleration = 4f;
	public float airDeceleration = 3f;

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
        // Gather input
        moveInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        // Animator and sprite updates (visual only)
        animator.SetBool("IsOnGround", isGrounded);
        animator.SetFloat("VelocityY", rb != null ? rb.velocity.y : 0f);
        animator.SetBool("IsJumping", currentJump == 1);
        animator.SetBool("IsDoubleJumping", currentJump == 2);
        animator.SetBool("Isrunning", Mathf.Abs(moveInput) > 0.01f);

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float move = moveInput;

        float targetSpeed = move * speed;
        float speedDifference = targetSpeed - rb.velocity.x;

        float accelRate;
        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = isGrounded ? acceleration : airAcceleration;
        }
        else
        {
            accelRate = isGrounded ? deceleration : airDeceleration;
        }

        float movement = speedDifference * accelRate;
        rb.AddForce(Vector2.right * movement);

        // Wall check and wall slide
        isTouchingWall = CheckWallContact(out wallDirection);
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0f)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        // Handle jumps (using flag set in Update)
        if (jumpPressed)
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

            jumpPressed = false;
        }
    }

    private bool CheckWallContact(out int direction)
    {
        direction = 0;
        Vector2 position = transform.position;

        RaycastHit2D rightHit = Physics2D.Raycast(position, Vector2.right, wallCheckDistance, groundLayer);
        if (rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            direction = -1;
            return true;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(position, Vector2.left, wallCheckDistance, groundLayer);
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
			CheckGroundCollision(collision);
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			CheckGroundCollision(collision);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;
		}
	}

	void CheckGroundCollision(Collision2D collision)
	{
		foreach (ContactPoint2D contact in collision.contacts)
		{
			// Normal pointing upward means ground is below player
			if (contact.normal.y > 0.5f)
			{
				isGrounded = true;
				jumpsRemaining = maxJumps;
				currentJump = 0;
				return;
			}
		}
	}
}
