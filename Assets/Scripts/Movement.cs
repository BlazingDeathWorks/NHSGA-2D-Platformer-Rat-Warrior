using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 1;
    private float x;
    private bool canMove = true;

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 1;
    [SerializeField] [field: FormerlySerializedAs("offsets")] private Transform[] groundRaycastPos;
    [SerializeField] [field: FormerlySerializedAs("raycastLength")] private float groundRaycastLength;
    [SerializeField] private LayerMask whatIsGround;
    RaycastHit2D[] groundRaycastHits;
    private const int MAX_JUMPS = 2;
    private int jumpCount;
    private bool canJump;
    private bool isDoubleJumping;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBuffer = 0.3f;
    private float timeSinceJumpPressed;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float timeSinceLeaveGround;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private float timeSinceLastDash;
    private bool canDash;

    [Header("Wall Sliding")]
    [SerializeField] private Transform wallSlideRaycastPos;
    [SerializeField] private float wallSlideRaycastLength;
    [SerializeField] private float kickoutHorizontalSpeed;
    private RaycastHit2D wallSlideRaycastHit;
    private bool canWallSlide;
    private bool isWallSliding;
    private bool isWallJumping;
    private float timeSinceWallJump;

    private bool canSprint;

    private float targetXPos;
    private float knockBackForce;
    private float timeSinceHit;
    private bool isHit;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundRaycastHits = new RaycastHit2D[groundRaycastPos.Length];

        timeSinceLastDash = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        EnableCanMove();

        DisplayRaycastLines();

        CheckJumpBuffer();

        CheckDash();

        if (canJump && isWallSliding)
        {
            canMove = false;
            jumpCount = 0;
            isWallJumping = true;
        }

        if (isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= 0.2f)
            {
                isHit = false;
                animator.SetBool("isHit", isHit);
                timeSinceHit = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        //Create the ground raycasts
        for (int i = 0; i < groundRaycastPos.Length; i++) groundRaycastHits[i] = Physics2D.Raycast(groundRaycastPos[i].position, Vector2.down, groundRaycastLength, whatIsGround);

        //Create the wall sliding raycast
        wallSlideRaycastHit = Physics2D.Raycast(wallSlideRaycastPos.position, Vector2.right * Mathf.Sign(transform.localScale.x), wallSlideRaycastLength, whatIsGround);

        if (isHit)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(1 * Mathf.Sign(transform.position.x - targetXPos), 0.3f).normalized * knockBackForce, ForceMode2D.Impulse);
            return;
        }

        //Horizontal Velocity
        if (canMove)
        {
            rb.velocity = canSprint ? new Vector2(x * speed * 2, rb.velocity.y) : new Vector2(x * speed, rb.velocity.y);
        }

        //Dashing
        if (canDash)
        {
            rb.velocity = new Vector2(dashSpeed * x, 0);
        }

        //Falling
        if (rb.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        if (rb.velocity.y <= -1)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        //Double Jump Animation
        if (isDoubleJumping)
        {
            animator.SetInteger("yVelocity", (int)Mathf.Sign(rb.velocity.y));
            isDoubleJumping = false;
            animator.SetBool("isDoubleJumping", isDoubleJumping);
        }

        CheckRaycasts();

        //Allowed to jump if on wall
        if (isWallJumping)
        {
            //canMove = false;
            //jumpCount = 0;
            //isWallSliding = false;
            Jump(new Vector2(kickoutHorizontalSpeed * -x, jumpPower));
        }

        //Allowed to jump in air
        if (canJump && jumpCount + 1 < MAX_JUMPS && !isWallSliding)
        {
            jumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            canJump = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            isDoubleJumping = true;
            animator.SetBool("isDoubleJumping", isDoubleJumping);
        }
    }

    #region Display Raycasts
    private void DisplayRaycastLines()
    {
        //Debugging Purposes
        for (int i = 0; i < groundRaycastPos.Length; i++) Debug.DrawLine(groundRaycastPos[i].position, (Vector2)groundRaycastPos[i].position + new Vector2(0, -groundRaycastLength));
        Debug.DrawLine(wallSlideRaycastPos.position, (Vector2)wallSlideRaycastPos.position + new Vector2(wallSlideRaycastLength * x, 0));
    }
    #endregion

    private void Jump(Vector2 jumpDirection)
    {
        rb.velocity = jumpDirection;
        canJump = false;
        animator.SetBool("isJumping", true);
    }

    private void CheckJumpBuffer()
    {
        //Don't need to run the bottom code if canJump == false
        if (canJump == false || isWallSliding) return;

        //Jump Buffer so we can press jump early and still jump once we hit the ground
        timeSinceJumpPressed += Time.deltaTime;
        if (timeSinceJumpPressed >= jumpBuffer)
        {
            canJump = false;
        }
    }

    private void CheckDash()
    {
        timeSinceLastDash += Time.deltaTime;

        if (canDash)
        {
            if (timeSinceLastDash >= dashTime)
            {
                canDash = false;
                timeSinceLastDash = 0;
            }
            return;
        }
    }

    private void EnableCanMove()
    {
        if (!isWallJumping) return;

        timeSinceWallJump += Time.deltaTime;
        if (timeSinceWallJump >= 0.01f)
        {
            timeSinceWallJump = 0;
            isWallJumping = false;
            canMove = true;
        }
    }

    private void CheckRaycasts()
    {
        //Check the below for each ground raycast
        for (int i = 0; i < groundRaycastHits.Length; i++)
        {
            //Return to ground
            if (groundRaycastHits[i])
            {
                jumpCount = 0;
                animator.SetBool("isFalling", false);
                timeSinceLeaveGround = 0;
                //Check Standard Jump
                if (canJump)
                {
                    Jump(new Vector2(rb.velocity.x, jumpPower));
                }
                continue;
            }
            canWallSlide = true;
        }

        if (canWallSlide)
        {
            timeSinceLeaveGround += Time.fixedDeltaTime;
            if (timeSinceLeaveGround < coyoteTime && canJump)
            {
                Jump(new Vector2(rb.velocity.x, jumpPower));
                timeSinceLeaveGround = coyoteTime;
            }
        }

        //Check Wall Sliding
        if (wallSlideRaycastHit && canWallSlide && rb.velocity.x != 0 && rb.velocity.y < 0)
        {
            isWallSliding = true;
            animator.SetBool("isWallSliding", isWallSliding);
            canWallSlide = false;
            return;
        }
        isWallSliding = false;
        animator.SetBool("isWallSliding", isWallSliding);
        canWallSlide = false;
    }

    public void TriggerHit(float knockBackForce, float targetXPos)
    {
        this.knockBackForce = knockBackForce;
        this.targetXPos = targetXPos;
        isHit = true;
        animator.SetBool("isHit", isHit);
    }

    #region New Input Events
    public void OnMove(InputAction.CallbackContext value)
    {
        if (isHit) return;
        x = value.ReadValue<Vector2>().x;

        if (x != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(x), transform.localScale.y, transform.localScale.z);
            animator.SetBool("isRunning", true);
            return;
        }
        animator.SetBool("isRunning", false);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (!value.ReadValueAsButton()) return;

        canJump = true;
        timeSinceJumpPressed = 0;
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton() && timeSinceLastDash >= dashCooldown)
        {
            timeSinceLastDash = 0;
            canDash = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            canSprint = true;
        }
        if (value.canceled)
        {
            canSprint = false;
        }
    }
    #endregion
}
