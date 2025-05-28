using UnityEngine;

public class Script : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xInput;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private int FlipDir = 1;
    private bool facingRight = true;

    [Header("Dash Info")]
    [SerializeField] private float dashDuration;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCoolDownTime;

    [Header("Attack Info")]
    [SerializeField] private bool isAttacking;

    [SerializeField] private int comboCounter;
    [SerializeField] private float comboTime = 0.6f;
    private float comboTimeWindow;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();
        dashTime -= Time.deltaTime;
        dashCoolDownTime -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        AnimationController();
        FlipController();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime >= 0)
        {
            rb.velocity = new Vector2(FlipDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        }
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolDownTime <= 0)
        {
            dashCoolDownTime = 2;
            dashTime = dashDuration;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
        {
            return;
        }

        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void AnimationController()
    {
        bool isMoving = rb.velocity.x != 0;
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isDashing", dashTime > 0);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetInteger("ComboCounter", comboCounter);
    }

    private void Flip()
    {
        FlipDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, FlipDir * 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }


    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }
}