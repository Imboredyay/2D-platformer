using UnityEngine;
using Mirror;
public class PlayerMovementMulti : NetworkBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float wallSlideSpeed = 1.5f;
    public float wallJumpHorizontalForce = 6f;
    public float wallJumpVerticalForce = 7f;
    public float wallCheckDistance = 0.6f;
	public LayerMask groundLayer;
	
	public float dashSpeed = 20f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 0.5f;

	private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isTouchingWall;
    private int wallDirection;
    private int jumpsRemaining;
    private int currentJump;
    private const int maxJumps = 1;


    private float moveInput;
    private bool jumpPressed;
    private bool dashPressed;
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private float dashDirection;

	public float acceleration = 20f;
	public float deceleration = 18f;
	public float airAcceleration = 12f;
	public float airDeceleration = 8f;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashPressed = true;
        }

        // Update cooldown timer
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
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

        // Handle dash
        if (dashPressed && !isDashing && dashCooldownTimer <= 0)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashDirection = moveInput != 0 ? moveInput : (spriteRenderer.flipX ? -1 : 1);
            dashCooldownTimer = dashCooldown;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        dashPressed = false;
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
