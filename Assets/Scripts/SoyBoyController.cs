using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class SoyBoyController : MonoBehaviour
{
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip slideClip;
    private AudioSource audioSource;

    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration = 0f;

    public float speed = 14f;
    public float accel = 6f;
    public float airAccel = 3f;

    public float jump = 14f;

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
        audioSource = GetComponent<AudioSource>();
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

        animator.SetFloat("Speed", Mathf.Abs(input.x));

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
            animator.SetBool("IsJumping", true);
        }
        else
        {
            isJumping = false;
            animator.SetBool("IsJumping", isJumping);
            jumpDuration = 0f;
        }

        if (PlayerIsOnGround() && !isJumping)
        {
            if (input.y > 0)
            {
                isJumping = true;
                PlayAudioClip(jumpClip);
            }

            animator.SetBool("IsOnWall", false);

            if (input.x < 0f || input.x > 0f)
            {
                PlayAudioClip(runClip);
            }
        }
    }

    void FixedUpdate()
    {
        float xVelocity = 0f;
        float yVelocity = 0f;

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

        if (PlayerIsTouchingGroundOrWall() && Mathf.Approximately(input.y, 1f))
        {
            yVelocity = jump;
        }
        else
        {
            yVelocity = body.velocity.y;
        }

        Vector2 force = new Vector2(((input.x * speed) - xVelocity) * acceleration, 0);
        body.AddForce(force);
        body.velocity = new Vector2(xVelocity, yVelocity);

        if (PlayerIsWallToLeftOrRight() && !PlayerIsOnGround() && Mathf.Approximately(input.y, 1f))
        {
            body.velocity = new Vector2(-GetWallDirection() * speed * 0.75f, body.velocity.y);
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);
            PlayAudioClip(jumpClip);
        }
        else if (!PlayerIsWallToLeftOrRight())
        {
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);      
        }

        if (PlayerIsWallToLeftOrRight() && !PlayerIsOnGround())
        {
            animator.SetBool("IsOnWall", true);
            PlayAudioClip(slideClip);
        }

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


    private bool PlayerIsLeftWall()
    {
        Vector2 origin = new Vector2(transform.position.x - width, transform.position.y);
        bool isLeftWall = Physics2D.Raycast(origin, Vector2.left, rayCastLengthCheck);

        if (isLeftWall)
        {
            return true;
        }

        return false;
    }

    private bool PlayerIsRightWall()
    {
        Vector2 origin = new Vector2(transform.position.x + width, transform.position.y);
        bool isRightWall = Physics2D.Raycast(origin, Vector2.right, rayCastLengthCheck);

        if (isRightWall)
        {
            return true;
        }

        return false;
    }

    public bool PlayerIsWallToLeftOrRight()
    {
        if (PlayerIsLeftWall() || PlayerIsRightWall())
        {
            return true;
        }

        return false;
    }

    public bool PlayerIsTouchingGroundOrWall()
    {
        if (PlayerIsOnGround() || PlayerIsWallToLeftOrRight())
        {
            return true;
        }

        return false;
    }

    public int GetWallDirection()
    {
        if (PlayerIsLeftWall())
        {
            return -1;
        }
        else if (PlayerIsRightWall())
        {
            return 1;
        }

        return 0;
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clip);    
            }
        }
    }
}
