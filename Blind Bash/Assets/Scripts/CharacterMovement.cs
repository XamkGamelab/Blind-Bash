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
    private PlayerStats stats; //player stats script ref.
    private Vector2 moveDirection = Vector2.zero; //current movement direction
    private bool canMove = true; //true = waiting for input, false = moving.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>(); //from children to allow free rotation of sprite.
        stats = GetComponent<PlayerStats>(); //init. stats for further use in collisions and such,
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
    }
    private void SetMoveDirection(Vector2 dir) //sets the movement direction and disables input until a wall is hit.
    {
        moveDirection = dir;
        canMove = false; //start moving until hit a wall

        stats.UseMove(); //call UseMove -method in PlayerStats to update movement count status.

        float angle = 0f; //angle to rotate the sprite to flip its facing according to the given mov. dir.
        if (dir == Vector2.up) angle = 0f; //by default the dog is facing up. (for now at least).
        else if (dir == Vector2.right) angle = -90f;
        else if (dir == Vector2.down) angle = 180f;
        else if (dir == Vector2.left) angle = 90f;

        sprite.transform.localRotation = Quaternion.Euler(0, 0, angle); //rotate the sprite
    }
    private void OnCollisionEnter2D(Collision2D collision) //collision handling.
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;

            if (Vector2.Dot(moveDirection, -collisionNormal) > 0.5f)
            {
                StopMovement(); //if moveDirection points into the wall, stop
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("DamageSource"))
        {
            stats.TakeDamage(1); //take damage when colliding with damage source tagged stuff
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {
            stats.AddScore(10); //increment score on collectible collision
            Destroy(collision.gameObject); //destroy collectible.
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
        speed=0; //set speed to 0 on collision for new acceleration.
        rb.velocity = Vector2.zero;
        moveDirection = Vector2.zero;
        canMove = true; //input enabled again.
    }
}
