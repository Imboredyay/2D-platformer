using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip deathSound;

	public float dashSpeed = 20f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 0.5f;

	private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
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

        // Handle jumps
        if (jumpPressed)
        {
            if (jumpsRemaining > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsRemaining--;
                currentJump++;
                SoundManager.Instance?.PlaySound(jumpSound);
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

private bool playedDeathSound;

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

    private void OnDestroy()
    {
        if (!playedDeathSound && Application.isPlaying)
        {
            playedDeathSound = true;
            SoundManager.Instance?.PlaySound(deathSound);
		}
	}
}
