using UnityEngine;

public class Script : Entity
{
    private float xInput;

    [Header("Move Info")]
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;


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

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        dashTime -= Time.deltaTime;
        dashCoolDownTime -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        AnimationController();
        FlipController();
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