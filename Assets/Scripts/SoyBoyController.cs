using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class SoyBoyController : MonoBehaviour
{
    public float speed = 14f;
    public float accel = 6f;

    private Vector2 input;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");

        if (input.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        float acceleration = accel;
        float xVelocity = 0f;

        if (Mathf.Approximately(input.x, 0f)) //(input.x == 0)
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
    }
}
