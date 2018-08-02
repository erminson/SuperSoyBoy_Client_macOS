using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class SoyBoyController : MonoBehaviour
{
    public float jumpDurationThreshold = 10.25f;
    private float jumpDuration = 0f;

    public float speed = 14f;
    public float accel = 6f;
    public float airAccel = 3f;

    private Vector2 input;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private Animator animator;

    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");
        Debug.Log(input.y);
        // Left & Right Move Control
        if (input.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < 0f)
        {
            spriteRenderer.flipX = true;
        }

        // Jump Control
        if (jumpDuration > jumpDurationThreshold)
        {
            input.y = 0f;
        }

        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;
        }
        else 
        {
            isJumping = false;
            jumpDuration = 0f;
        }

        if (PlayerIsOnGround() && !isJumping)
        {
            if (input.y > 0)
            {
                isJumping = true;
            }
        }
    }

    void FixedUpdate()
    {
        float xVelocity = 0f;

        float acceleration = 0f;
        if (PlayerIsOnGround())
        {
            acceleration = accel;    
        }
        else
        {
            acceleration = airAccel;
        }

        if (PlayerIsOnGround() && Mathf.Approximately(input.x, 0f)) //(input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = body.velocity.x;
        }
            
        Vector2 force = new Vector2(((input.x * speed) - xVelocity) * acceleration, 0);
        body.AddForce(force);
        body.velocity = new Vector2(xVelocity, body.velocity.y);

        if (isJumping && jumpDuration < jumpDurationThreshold)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    public bool PlayerIsOnGround()
    {
        Vector2 origin1 = new Vector2(transform.position.x, transform.position.y - height);
        bool groundCheck1 = Physics2D.Raycast(origin1, -Vector2.up, rayCastLengthCheck);

        Vector2 origin2 = new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height);
        bool groundCheck2 = Physics2D.Raycast(origin2, -Vector2.up, rayCastLengthCheck);

        Vector2 origin3 = new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height);
        bool groundCheck3 = Physics2D.Raycast(origin3, -Vector2.up, rayCastLengthCheck);

        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }

        return false;
    }
}
