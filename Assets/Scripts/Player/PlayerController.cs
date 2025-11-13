using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public int maxJumps = 1; // 1 = single jump; set 2 for double-jump
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    float horizontal;
    bool facingRight = true;
    int jumpsLeft;
    float lastGroundedTime;
    float lastJumpPressedTime;

    [Header("Animation")]
    public Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Input buffering
        if (Input.GetButtonDown("Jump"))
            lastJumpPressedTime = Time.time;

        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            jumpsLeft = maxJumps;
        }

        // Jump conditions: coyote time or extra jumps
        if (Time.time - lastJumpPressedTime <= jumpBufferTime)
        {
            if (Time.time - lastGroundedTime <= coyoteTime || jumpsLeft > 0)
            {
                DoJump();
                lastJumpPressedTime = -999f;
            }
        }

        // Flip sprite
        if (horizontal > 0.1f && !facingRight) Flip();
        else if (horizontal < -0.1f && facingRight) Flip();

        // ✨ Update animation parameters
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset Y velocity for consistent jumps
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsLeft = Mathf.Max(0, jumpsLeft - 1);
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        Collider2D col = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return col != null;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
