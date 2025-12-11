using UnityEngine;
using UnityEngine.InputSystem;

// Handles all player movement, jumping, wall interactions, animations, audio, and interactions.
// Supports coyote time, jump buffering, wall jumps, wall slides, and advanced gravity.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;                  // Horizontal speed
    public float groundAccelerationTime = 0.05f;  // Smooth acceleration on ground
    public float airAccelerationTime = 0.12f;     // Smooth acceleration in air
    public float jumpVelocity = 14f;
    public int maxJumps = 1;                      // Number of jumps allowed
    public float coyoteTime = 0.12f;              // Grace period after leaving ground
    public float jumpBufferTime = 0.12f;          // Buffer for jump input
    public float maxJumpHoldTime = 0.18f;         // Max duration to hold jump
    public float normalGravityScale = 1f;
    public float lowJumpMultiplier = 2.2f;        // Gravity when jump released early
    public float fallMultiplier = 2.2f;           // Gravity multiplier for falling
    public float fastFallMultiplier = 1.4f;       // Extra gravity when pressing down
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 0.1f;
    public float wallSlideSpeedMax = 2.5f;
    public float wallStickDuration = 0.18f;
    public Vector2 wallJumpVelocity = new Vector2(10f, 14f);
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float interactRange = 1f;
    public LayerMask interactableLayer;
    public Animator animator;
    public AudioClip runningLoopSound;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float horizontalInput;
    private int jumpsLeft;
    private float lastGroundedTime = -999f;
    private float lastJumpPressedTime = -999f;
    private float jumpHoldTimer;
    private float velocityXSmoothing;
    private float currentSpeed;
    private bool isTouchingWall, isWallSliding;
    private int wallDirection;
    private float wallStickTimer;
    private bool wasGroundedLastFrameForSound, wasWallSlidingLastFrame;
    private IInteractable currentInteractable;

    private AudioSource footstepLoopSource;
    private AudioSource wallSlideSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = normalGravityScale;
    }
    
    void Start()
    {
        jumpsLeft = maxJumps;
        wallStickTimer = wallStickDuration;

        // Create looping audio sources for footstep and wall slide
        footstepLoopSource = CreateAudioSource("FootstepLoop", runningLoopSound, true);
        wallSlideSource = CreateAudioSource("WallSlideSource", null, true);
    }

    AudioSource CreateAudioSource(string name, AudioClip clip, bool loop)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        AudioSource src = obj.AddComponent<AudioSource>();
        src.clip = clip;
        src.loop = loop;
        src.playOnAwake = false;
        return src;
    }
    void Update()
    {
        HandleInput();
        DetectGroundAndWall();
        HandleJumpBuffer();
        HandleWallSlide();
        HandleFootsteps();
        HandleLandingSound();
        HandleWallSlideSound();
        UpdateAnimator(currentSpeed);
        FlipIfNeeded();
        CheckForInteractable();
        HandleInteractInput();

        UpdateAudioVolumes();
    }

    void UpdateAudioVolumes()
    {
        if (AudioManager.Instance != null)
        {
            float master = AudioManager.Instance.masterVolume;
            float sfx = AudioManager.Instance.sfxVolume;

            footstepLoopSource.volume = master * sfx * AudioManager.Instance.GetFootstepVolume();
            wallSlideSource.volume = master * sfx * AudioManager.Instance.GetWallSlideVolume();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyGravityModifiers();
    }

    void HandleFootsteps()
    {
        if (IsGrounded() && Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (!footstepLoopSource.isPlaying) footstepLoopSource.Play();
        }
        else
        {
            if (footstepLoopSource.isPlaying) footstepLoopSource.Stop();
        }
    }

    void HandleLandingSound()
    {
        if (IsGrounded() && !wasGroundedLastFrameForSound)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLandSound();
        }
        wasGroundedLastFrameForSound = IsGrounded();
    }

    void HandleWallSlideSound()
    {
        if (isWallSliding && !wasWallSlidingLastFrame)
        {
            if (AudioManager.Instance != null && AudioManager.Instance.wallSlideSound != null)
            {
                wallSlideSource.clip = AudioManager.Instance.wallSlideSound;
                wallSlideSource.Play();
            }
        }
        else if (!isWallSliding && wasWallSlidingLastFrame)
        {
            wallSlideSource.Stop();
        }
        wasWallSlidingLastFrame = isWallSliding;
    }

    void PerformJump(bool isWallJumpAttempt = false)
    {
        // Play jump sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayJumpSound();

        if (isWallJumpAttempt)
        {
            int dir = wallDirection == 0 ? (facingRight ? 1 : -1) : wallDirection;
            rb.linearVelocity = new Vector2(-dir * Mathf.Abs(wallJumpVelocity.x), wallJumpVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        jumpHoldTimer = maxJumpHoldTime;
        jumpsLeft = Mathf.Max(0, jumpsLeft - 1);
    }

    public void PlayDeathSound()
    {
        footstepLoopSource.Stop();
        wallSlideSource.Stop();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDeathSound();
    }

    void CheckForInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);
        currentInteractable = hit ? hit.GetComponent<IInteractable>() : null;
    }

    void HandleInteractInput()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            currentInteractable?.Interact();
    }

    bool IsGrounded() => groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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

    void HandleInput()
    {
        horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) horizontalInput = -1f;
            if (Keyboard.current.dKey.isPressed) horizontalInput = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                lastJumpPressedTime = Time.time;

            if (Keyboard.current.spaceKey.isPressed)
                jumpHoldTimer = maxJumpHoldTime;
        }
    }

    void DetectGroundAndWall()
    {
        bool grounded = IsGrounded();
        if (grounded)
        {
            lastGroundedTime = Time.time;
            jumpsLeft = maxJumps;
        }

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

    void UpdateAnimator(float horizontalVelocity)
    {
        bool grounded = IsGrounded();
        float yVel = rb.linearVelocity.y;

        animator.SetBool("IsGrounded", grounded);
        animator.SetBool("IsJumping", yVel > 0.1f);
        animator.SetBool("IsFalling", yVel < -0.1f);
        animator.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
    }

    void HandleMovement()
    {
        float targetVelX = horizontalInput * moveSpeed;
        float accelTime = IsGrounded() ? groundAccelerationTime : airAccelerationTime;

        float newVX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelX, ref velocityXSmoothing, accelTime);
        rb.linearVelocity = new Vector2(newVX, rb.linearVelocity.y);

        currentSpeed = Mathf.Abs(newVX);
    }

    void ApplyGravityModifiers()
    {
        bool holdingJump = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        bool pressingDown = Keyboard.current != null &&
                            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed);

        if (rb.linearVelocity.y < 0f)
        {
            float mult = fallMultiplier * (pressingDown ? fastFallMultiplier : 1f);
            rb.gravityScale = normalGravityScale * mult;
        }
        else if (rb.linearVelocity.y > 0f)
        {
            if (holdingJump && jumpHoldTimer > 0f)
                rb.gravityScale = normalGravityScale;
            else
                rb.gravityScale = normalGravityScale * lowJumpMultiplier;

            if (holdingJump) jumpHoldTimer -= Time.fixedDeltaTime;
        }
        else rb.gravityScale = normalGravityScale;
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
