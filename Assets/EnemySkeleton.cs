using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    private bool isAttacking;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;

    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        print(playerCheckDistance);
        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 2)
            {
                rb.velocity = new Vector2(moveSpeed * FlipDir * 1.5f, rb.velocity.y);
                print("I see the player");
                isAttacking = false;
            } else
            {
                Debug.Log("Attack" + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }

        if (!isGrounded || isWallDetected)
        {
            Flip();
        }

        Movement();
    }

    private void Movement()
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveSpeed * FlipDir, rb.velocity.y);
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected =
            Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * FlipDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + playerCheckDistance * FlipDir, transform.position.y));
    }
}