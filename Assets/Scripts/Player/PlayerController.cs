using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    [Tooltip("Time to reach target speed on ground (lower = snappier)")]
    public float groundAccelerationTime = 0.05f;
    [Tooltip("Time to reach target speed in air (higher = floatier)")]
    public float airAccelerationTime = 0.12f;

    [Header("Jump")]
    public float jumpVelocity = 14f;
    public int maxJumps = 1;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;
    public float maxJumpHoldTime = 0.18f;

    [Header("Gravity & Fall")]
    public float normalGravityScale = 1f;
    public float lowJumpMultiplier = 2.2f;
    public float fallMultiplier = 2.2f;
    public float fastFallMultiplier = 1.4f;

    [Header("Wall")]
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 0.1f;
    public float wallSlideSpeedMax = 2.5f;
    public float wallStickDuration = 0.18f;
    public Vector2 wallJumpVelocity = new Vector2(10f, 14f);

    [Header("Ground")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    // runtime
    Rigidbody2D rb;
    float horizontalInput;
    bool facingRight = true;
    private bool hasReachedApex = false;

    // jump state
    int jumpsLeft;
    [HideInInspector] public float lastGroundedTime = -999f;
    [HideInInspector] public float lastJumpPressedTime = -999f;
    [SerializeField] private float landingHoldTime = 0.2f; // duration for frames 6-7
    private float landingTimer = 0f;
    float jumpHoldTimer = 0f;
    bool wasGroundedLastFrame = false;

    // smoothing
    float velocityXSmoothing;
    float currentSpeed; // smoothed speed for animation

    // wall state
    bool isTouchingWall;
    int wallDirection; // -1 left, 1 right
    bool isWallSliding;
    float wallStickTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = normalGravityScale;
    }

    void Start()
    {
        jumpsLeft = maxJumps;
        wallStickTimer = wallStickDuration;
    }

    void Update()
    {
        HandleInput();
        DetectGroundAndWall();
        HandleJumpBuffer();
        HandleWallSlide();
        UpdateAnimator(currentSpeed);
        FlipIfNeeded();
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyGravityModifiers();
    }

    void HandleInput()
    {
        horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) horizontalInput = -1f;
            if (Keyboard.current.dKey.isPressed) horizontalInput = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                lastJumpPressedTime = Time.time;

            if (Keyboard.current.spaceKey.isPressed && Time.time - lastJumpPressedTime <= 0.02f)
                jumpHoldTimer = maxJumpHoldTime;
        }
    }

    void DetectGroundAndWall()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            jumpsLeft = maxJumps;
        }
        wasGroundedLastFrame = isGrounded;

        isTouchingWall = false;
        wallDirection = 0;
        if (wallCheck != null)
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);
            if (hitLeft.collider != null) { isTouchingWall = true; wallDirection = -1; }
            else if (hitRight.collider != null) { isTouchingWall = true; wallDirection = 1; }
        }
    }

    void HandleJumpBuffer()
    {
        bool coyoteActive = (Time.time - lastGroundedTime) <= coyoteTime;
        bool jumpBuffered = (Time.time - lastJumpPressedTime) <= jumpBufferTime;

        if (jumpBuffered)
        {
            if (coyoteActive || jumpsLeft > 0 || isTouchingWall)
            {
                PerformJump(isWallJumpAttempt: isTouchingWall && !IsGrounded());
                lastJumpPressedTime = -999f;
            }
        }
    }

    void HandleWallSlide()
    {
        isWallSliding = false;
        if (isTouchingWall && !IsGrounded() && horizontalInput == wallDirection)
        {
            if (wallStickTimer > 0f)
            {
                isWallSliding = true;
                wallStickTimer -= Time.deltaTime;
                if (rb.linearVelocity.y < -wallSlideSpeedMax)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeedMax);
            }
        }
        else wallStickTimer = wallStickDuration;
    }

    void HandleMovement()
    {
        float targetVelX = horizontalInput * moveSpeed;
        float accelTime = IsGrounded() ? groundAccelerationTime : airAccelerationTime;
        float vx = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelX, ref velocityXSmoothing, accelTime);
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        currentSpeed = Mathf.Abs(vx);
    }

    void PerformJump(bool isWallJumpAttempt = false)
    {
        if (isWallJumpAttempt)
        {
            int dir = wallDirection != 0 ? wallDirection : (facingRight ? 1 : -1);
            float jumpDirX = -dir * Mathf.Abs(wallJumpVelocity.x);
            rb.linearVelocity = new Vector2(jumpDirX, wallJumpVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        jumpHoldTimer = maxJumpHoldTime;
        jumpsLeft = Mathf.Max(0, jumpsLeft - 1);
    }

    void ApplyGravityModifiers()
    {
        bool holdingJump = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        bool pressingDown = Keyboard.current != null &&
                            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed);

        if (rb.linearVelocity.y < 0f)
        {
            float multiplier = fallMultiplier * (pressingDown ? fastFallMultiplier : 1f);
            rb.gravityScale = normalGravityScale * multiplier;
        }
        else if (rb.linearVelocity.y > 0f)
        {
            rb.gravityScale = holdingJump && jumpHoldTimer > 0f ? normalGravityScale : normalGravityScale * lowJumpMultiplier;
            if (holdingJump) jumpHoldTimer -= Time.fixedDeltaTime;
        }
        else rb.gravityScale = normalGravityScale;
    }

    void UpdateAnimator(float horizontalVelocity)
    {
        bool isGrounded = IsGrounded();
        float verticalVelocity = rb.linearVelocity.y;

        // Jumping (including wall jump)
        animator.SetBool("IsJumping", !isGrounded && verticalVelocity > 0.1f);

        // Falling
        animator.SetBool("IsFalling", !isGrounded && verticalVelocity < -0.1f);

        // Speed and ground
        animator.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
        animator.SetBool("IsGrounded", isGrounded);
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    void FlipIfNeeded()
    {
        if (horizontalInput > 0.1f && !facingRight) Flip();
        else if (horizontalInput < -0.1f && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
        }
    }
}
