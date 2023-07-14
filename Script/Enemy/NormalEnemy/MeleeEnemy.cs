using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Audio")]
    [SerializeField] private AudioClip meleehitSound;

    //References
    private Animator anim;
    private Health playerHealth;

    //combatAI
    [Header("CombatAI")]
    [SerializeField] private float blockCooldown = 1.5f;
    //[SerializeField] private float counterAttackDelay = 0.5f;
    public bool isBlocking = false;
    private bool isCounterAttacking = false;
    //public bool blockingSucceed = false;
        
    private EnemyPatroll enemyPatrol;
    private PlayerAttack playerAttack;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatroll>();

        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInAttackRange())
        {
            //Only when player insight?
            if (!isBlocking && !isCounterAttacking) {
                //Only when enemy do nothing
                if (cooldownTimer >= attackCooldown)
                {
                    float randomValue = Random.value;

                    if (randomValue < 0.5f)
                    {
                        //Attack
                        cooldownTimer = 0;
                        anim.SetTrigger("attack");

                        SoundManager.instance.PlaySound(meleehitSound);
                    }

                    else
                    {
                        // Hold the shield for block
                        isBlocking = true;
                        anim.SetBool("block", true);
                        Invoke(nameof(StopBlocking), blockCooldown);
                    }
                }
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInAttackRange();
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        //if Player still in hitbox
        if (PlayerInAttackRange())
        {
            //Damage player health
            playerHealth.TakeDamage(damage);
        }
    }

    public void Deactivate()
    {
        sr.enabled = false;
        boxCollider.enabled = false;
    }

    public void Activate()
    {
        sr.enabled = true;
        boxCollider.enabled = true;
    }

    private void StopBlocking()
    {
        isBlocking = false;
        anim.SetBool("block", false);
    }

    /*
    private void CounterAttack()
    {
        if (isBlocking && playerAttack.isAttacking)
        {
            // Perform a counter attack
            anim.SetTrigger("heavy");
            isCounterAttacking = true;
            Invoke(nameof(StopCounterAttack), counterAttackDelay);

            damage = damage * 1.5f;

            cooldownTimer = 0;

            blockingSucceed = true;

            Debug.Log("Blocking Succeed");
        }
        else
        {
            blockingSucceed = false;
        }
    }
    */

    private void StopCounterAttack()
    {
        isCounterAttacking = false;
    }
}