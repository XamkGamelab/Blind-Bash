using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    public float maxSpeed = 30f; //max. movement speed.
    public float acceleration = 5.5f; //acc. multiplier

    public float speed = 0f; // starting speed
    private Vector2 moveDirection = Vector2.zero; //current movement direction
    private bool canMove = true; //true = waiting for input, false = moving.
    private KeyCode lastKeyPressed; //for restricting movement through walls.

    private SpriteRenderer sprite; //sprite renderer ref.
    private PlayerStats stats; //player stats script ref.
    private Animator animator; //animator ref.
    private Rigidbody2D rb; //rigidbody2D ref.

    private Tilemap levelTilemap; //tilemap ref.


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>(); 
    }

    public void SetLevelTilemap(Tilemap tilemap)
    {
        levelTilemap = tilemap;
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
    }

    private void FixedUpdate()
    {
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
        speed += acceleration * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        rb.velocity = moveDirection * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision) //collision handling.
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (isBlockingCollision(collision))
            {
                StopMovement();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //When player starts moving while already touching a wall
        if (!canMove && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (isBlockingCollision(collision))
            {
                StopMovement();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamageSource"))
        {
            stats.TakeDamage(1); //take damage when colliding with damage source tagged stuff
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {
            AudioManager.Instance.PlaySFX("toy");
            stats.AddScore(10); //increment score on collectible collision
            Destroy(collision.gameObject); //destroy collectible.
        }
    }

    //Only collision in the front is considered to block movement
    private bool isBlockingCollision(Collision2D collision)
    {
        if (moveDirection == Vector2.zero)
        {
            return false;
        }

        foreach (var contact in collision.contacts)
        {
            Vector2 normal = contact.normal; //From wall to player

            //Moving right: wall must face strongly left
            if (moveDirection == Vector2.right && normal.x < -0.9f)
            {
                return true;
            }

            //Moving left: wall must face strongly right
            if (moveDirection == Vector2.left && normal.x > 0.9f)
            {
                return true;
            }

            //Moving up: wall must face strongly down
            if (moveDirection == Vector2.up && normal.y < -0.9f)
            {
                return true;
            }

            //Moving down: wall must face strongly up
            if (moveDirection == Vector2.down && normal.y > 0.9f)
            {
                return true;
            }
        }
        return false;
    }

    private void StopMovement()
    {
        speed=0f; //set speed to 0 on collision for new acceleration.
        rb.velocity = Vector2.zero; //Just in case lmao
        canMove = true; //input enabled again.
        lastKeyPressed = KeyCode.None; //clears the lastKeyPressed

        //Fully snap to the tile center when you stop
        Vector3Int cell = levelTilemap.WorldToCell(transform.position);
        Vector3 cellCenter = levelTilemap.GetCellCenterWorld(cell);
        rb.position = cellCenter;

        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Crash");
    }
}
