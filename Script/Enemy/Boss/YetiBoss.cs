using UnityEngine;
using System.Collections;

public class YetiBoss : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip landedSound;

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private float Jumppower;
    [SerializeField] private float chargeSpeed = 5f;
    [SerializeField] private float chargeDuration;

    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage = 150;


    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask inviWallLayer;

    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    public bool detectPlayer { get; private set; }
    private bool isFlipped;
    private float targetX;
    private bool isCharging;
    private float speed;

    private Health playerHealth;

    private float cooldownTimer = Mathf.Infinity;
    private Vector3 defaultPosition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        speed = baseSpeed;
        defaultPosition = new Vector3(35.48f, transform.position.y, transform.position.z);


        isFlipped = false;
        isCharging = false;
        detectPlayer = false;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (player.position.x < 0.1f)
        {
            ResetToDefaultState();
        }

        if (!isGrounded() || !isLanded())
        {
            anim.SetBool("jump", true);
        }
        if (isGrounded() || isLanded())
        {
            anim.SetBool("jump", false);
        }

        if (isLanded())
        {
            LandedDamage();
        }

        if (player.position.x >= 27 && !detectPlayer)
        {
            DetectPlayer();
        }

        if (detectPlayer)
        {
            LookAtPlayer();

            //Track player's X position -> to make the boss dont collab with the player!
            targetX = player.position.x;


            // Move towards the player            
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), speed * Time.deltaTime);
            if (!isGrounded() || !isLanded())
            {
                anim.SetBool("walk", false);
            }
            if (isGrounded() || isLanded())
            {
                anim.SetBool("walk", true);
            }


            if (cooldownTimer >= attackCooldown)
            {
                float randomValue = Random.value;

                if (randomValue < 0.5f && !isCharging)
                {
                    cooldownTimer = 0;
                    Jump();
                }
                else
                {
                    cooldownTimer = 0;
                    Charge();
                }
            }

        }

    }

    #region Jump animation
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, Jumppower);
        anim.SetBool("jump", true);
        
    }

    private void LandedDamage()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    // Handle collision with player during boss landing
                    playerHealth.TakeDamage(damage);
                    SoundManager.instance.PlaySound(landedSound);

                    // Disable collision between the boss and player layer
                    Physics2D.IgnoreCollision(boxCollider, collider);
                    StartCoroutine(EnableCollisionAfterDelay(boxCollider, collider, 0.2f)); // Adjust delay as needed
                    break;
                }
            }
        }
    }

    private IEnumerator EnableCollisionAfterDelay(Collider2D bossCollider, Collider2D playerCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(bossCollider, playerCollider, false);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isLanded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, playerLayer);
        return raycastHit.collider != null;
    }
    #endregion

    private void DetectPlayer()
    {
        anim.SetTrigger("idle");
        detectPlayer = true;
    }

    #region Charge animation
    public void Charge()
    {
        if (!isCharging)
        {
            StartCoroutine(ChargeCoroutine());
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        isCharging = true;
        speed = chargeSpeed;
        anim.SetBool("charge", true);

        yield return new WaitForSeconds(chargeDuration);

        StopCharge();
    }

    private void StopCharge()
    {
        speed = baseSpeed;
        isCharging = false;
        anim.SetBool("charge", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && anim.GetCurrentAnimatorStateInfo(0).IsName("Charge"))
        {
            // Handle collision with player
            playerHealth.TakeDamage(damage);
            SoundManager.instance.PlaySound(landedSound);
            StopCharge();
            Debug.Log("Hit the player!");
        }
    }
    #endregion
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private bool onInviWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, inviWallLayer);
        return raycastHit.collider != null;
    }

    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    private void ResetToDefaultState()
    {
        // Reset boss position
        transform.position = defaultPosition;

        // Reset other boss properties and states

        // Stop any active coroutines
        StopAllCoroutines();

        // Reset animations and parameters
        anim.SetTrigger("idle");
        anim.SetBool("walk", false);
        anim.SetBool("jump", false);
        anim.SetBool("charge", false);

        // Reset other variables and flags
        isFlipped = false;
        detectPlayer = false;
        isCharging = false;
        cooldownTimer = Mathf.Infinity;
        // Reset other components or states as needed

        // Enable collision with player
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(boxCollider, playerCollider, false);
    }
}
