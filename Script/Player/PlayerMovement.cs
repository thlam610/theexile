using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float Jumppower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip healingSound;

    [Header("Healing System")]
    [SerializeField] private float HealAmount;
    [SerializeField] private int MaxNoPotions;
    private int CurrentNoPotions;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerAttack playerAttack;
    private Health playerHealth;
    private float HorizontalInput;
    private bool isCrouching = false;
    private bool isDashing = false;
    private float dashTimer;
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float acceleration = 10f;

    // Start is called before the first frame update
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();

        playerAttack = GetComponent<PlayerAttack>();

        playerHealth = GetComponent<Health>();

        CurrentNoPotions = MaxNoPotions;
    }

    // Update is called once per frame
    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        if (HorizontalInput != 0 && isCrouching)
        {
            if(HorizontalInput != 0 && playerAttack.isBlocking)
            body.velocity = new Vector2(0, body.velocity.y);
        }
        else if (HorizontalInput != 0 && !isCrouching && !playerAttack.isBlocking)
        {
            body.velocity = new Vector2(HorizontalInput * speed, body.velocity.y);
        }
        //Flip player
        if (HorizontalInput > 0.01f)
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);

        //Jump condition
        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        //Denied Wall climbing
        if (onWall())
        {
            body.gravityScale = 5;
            body.velocity = new Vector2(0, body.velocity.y);
        }
        //Set run animation
        anim.SetBool("run", HorizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Set crouch animation
        // Handle input for crouch
        if (Input.GetKey(KeyCode.LeftControl) && !isCrouching)
        {
            isCrouching = true;
            Crouch();
        }

        else if (!Input.GetKey(KeyCode.LeftControl) && isCrouching)
        {
            isCrouching = false;
            StopCrouching();
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isDashing && HorizontalInput != 0)
        {
            isDashing = true;
            anim.SetBool("dash", true);
            dashTimer = dashDuration;
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !isCrouching && !isDashing && canAttack() && canBlock())
        {
            Healing();
        }

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            //Move the character during dash
            float dashSpeed = Mathf.Sign(HorizontalInput) * dashDistance / dashDuration;
            body.velocity = new Vector2(dashSpeed, body.velocity.y);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                anim.SetBool("dash", false);
                body.velocity = Vector2.zero;
            }
            else
            {
                float currentSpeed = body.velocity.x;
                float deceleration = -Mathf.Sign(currentSpeed) * acceleration;
                body.AddForce(new Vector2(deceleration, 0f));
            }
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, Jumppower);
        anim.SetTrigger("jump");
        // Adjust the colliders
        SoundManager.instance.PlaySound(jumpSound);
    }

    private void Crouch()
    {
        // Play crouch animation
        anim.SetBool("crouch", true);
        // Adjust the colliders
    }

    private void StopCrouching()
    {
        // Stop crouch animation
        anim.SetBool("crouch", false);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private bool stickonWall()
    {
        return onWall() && !isGrounded();
    }

    public bool canAttack()
    {
        return !stickonWall();
    }

    public bool canJumpAttack()
    {
        return !isGrounded();
    }

    public bool canBlock()
    {
        return isGrounded() && !isCrouching;
    }

    private void Healing()
    {
        if (CurrentNoPotions > 0)
        {
            SoundManager.instance.PlaySound(healingSound);
            playerHealth.AddHealth(HealAmount);

            CurrentNoPotions--;
        }

        else return;
    }
}