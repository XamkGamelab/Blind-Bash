using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// added a random comment for classroom activity ~Tuomas
public class CharacterMovement : MonoBehaviour
{
    public float maxSpeed = 30; //movement speed.
    public float speed = 0; // starting speed
    public float acceleration = 5.5f; //acc. multiplier

    private Rigidbody2D rb; //rigidbody reference
    private SpriteRenderer sprite; //sprite renderer ref.
    private Vector2 moveDirection = Vector2.zero; //current movement direction
    private bool canMove = true; //true = waiting for input, false = moving.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
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
            if (speed < maxSpeed)
            {
                speed += acceleration*Time.deltaTime;
            }
            rb.velocity = moveDirection * speed;
        }
        /*
        else
        {
            rb.velocity = Vector2.zero; //stay still until next input.
        }
        */
       
    }
    private void SetMoveDirection(Vector2 dir) //sets the movement direction and disables input until a wall is hit.
    {
        moveDirection = dir;
        canMove = false; //start moving until hit a wall

        //flip sprite (maybe this'll be finished if it is necessary. Currently just flips image from left to right). UNDRED CONSTRUCTION.
        if (dir == Vector2.up)
            sprite.transform.localScale = new Vector3(1, 1, 1);
        else if (dir == Vector2.down)
            transform.localScale = new Vector3(1, -1, 1);
        else if (dir == Vector2.left)
            sprite.transform.Rotate(0, 0, -90);
        else if (dir == Vector2.right)
            sprite.transform.Rotate(0, 0, -90);
            
            
    }
    private void OnCollisionEnter2D(Collision2D collision) //called when colliding with a wall. Stops movement ONLY if the wall is blocking the current direction.
    {
        speed=0;

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
