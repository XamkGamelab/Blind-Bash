using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f; //movement speed.

    private Rigidbody2D rb; //rigidbody reference
    private Vector2 moveDirection = Vector2.zero; //current movement direction
    private bool canMove = true; //true = waiting for input, false = moving.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canMove) //only read input if we are allowed to move (not currently moving).
        {
            if (Input.GetKeyDown(KeyCode.W))
                SetMoveDirection(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.S))
                SetMoveDirection(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.A))
                SetMoveDirection(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.D))
                SetMoveDirection(Vector2.right);
        }
    }

    void FixedUpdate()
    {
        // Apply velocity if currently moving
        if (!canMove)
        {
            rb.velocity = moveDirection * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; //stay still until next input.
        }
    }
    private void SetMoveDirection(Vector2 dir) //sets the movement direction and disables input until a wall is hit.
    {
        moveDirection = dir;
        canMove = false; //start moving until hit a wall

        //flip sprite (maybe this'll be finished if it is necessary. Currently just flips image from left to right).
        if (dir == Vector2.left)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir == Vector2.right)
            transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnCollisionEnter2D(Collision2D collision) //called when colliding with a wall. Stops movement ONLY if the wall is blocking the current direction.
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;

            if (Vector2.Dot(moveDirection, -collisionNormal) > 0.5f)
            {
                StopMovement(); //if our moveDirection points into the wall, stop
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //extra safety check in case of continuous collision (like sliding along walls).
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;

            if (Vector2.Dot(moveDirection, -collisionNormal) > 0.5f)
            {
                StopMovement();
            }
        }
    }
    private void StopMovement() //stops the rigidbody and re-enables input.
    {
        rb.velocity = Vector2.zero;
        moveDirection = Vector2.zero;
        canMove = true; //input enabled again.
    }
}
