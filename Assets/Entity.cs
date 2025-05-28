using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    protected Rigidbody2D rb;
    protected Animator animator;
    protected int FlipDir = 1;
    protected bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;

    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Space]
    [SerializeField] protected Transform wallCheck;

    [SerializeField] protected float wallCheckDistance;

    protected bool isWallDetected;
    protected bool isGrounded;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        if (wallCheck == null)
        {
            wallCheck = transform;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected =
            Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * FlipDir, whatIsGround);
    }

    protected virtual void Flip()
    {
        FlipDir *= -1;
        facingRight = !facingRight;
        print(FlipDir);
        transform.Rotate(0, FlipDir * 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * FlipDir, wallCheck.position.y));
    }
}