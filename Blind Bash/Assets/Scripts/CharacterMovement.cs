using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    public float maxSpeed = 30f; //max. movement speed.
    public float acceleration = 5.5f; //acc. multiplier

    public float speed = 0; // starting speed
    private Vector2 moveDirection = Vector2.zero; //current movement direction
    private bool canMove = true; //true = waiting for input, false = moving.
    private KeyCode lastKeyPressed; //for restricting movement through walls.

    private SpriteRenderer sprite; //sprite renderer ref.
    private PlayerStats stats; //player stats script ref.
    private Animator animator; //animator ref.


    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>(); 
    }

    void Update()
    {
        if (canMove) //only read input if we are allowed to move (not currently moving).
        {
            if (Input.GetKeyDown(KeyCode.W) && lastKeyPressed != KeyCode.W)
            {
                lastKeyPressed = KeyCode.W;
                SetMoveDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S) && lastKeyPressed != KeyCode.S)
            {
                lastKeyPressed = KeyCode.S;
                SetMoveDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.A) && lastKeyPressed != KeyCode.A)
            {
                lastKeyPressed = KeyCode.A;
                SetMoveDirection(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) && lastKeyPressed != KeyCode.D)
            {
                lastKeyPressed = KeyCode.D;
                SetMoveDirection(Vector2.right);
            }
        }

        if (!canMove)
        {
            ApplyMovement();
        }
    }
   
    private void SetMoveDirection(Vector2 dir) //sets the movement direction and disables input until a wall is hit.
    {
        animator.SetBool("IsRunning", true);
        canMove = false; //start moving until hit a wall
        moveDirection = dir;
        
        float angle = 0f; //angle to rotate the sprite to flip its facing according to the given mov. dir.
        if (dir == Vector2.up) angle = 0f; //by default the dog is facing up. (for now at least).
        else if (dir == Vector2.right) angle = -90f;
        else if (dir == Vector2.down) angle = 180f;
        else if (dir == Vector2.left) angle = 90f;

        sprite.transform.localRotation = Quaternion.Euler(0, 0, angle); //rotate the sprite
    }

    private void ApplyMovement()
    {
        speed += acceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision) //collision handling.
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopMovement();
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

    private void StopMovement()
    {
        speed=0f; //set speed to 0 on collision for new acceleration.
        canMove = true; //input enabled again.
        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Crash");
    }
}
