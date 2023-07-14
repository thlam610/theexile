using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attack")]
    [SerializeField] private float AttackCooldown;
    [SerializeField] public float damage = 80;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 0.8f;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Spell")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] Fireballs;


    [Header("Audio")]
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip meleehitSound;

    [Header("References")]
    [SerializeField] private BossHealth bossHealth;


    private Animator anim;
    private PlayerMovement playerMovement;
    private Health playerHealth;
    private EnemyHealth enemyHealth;
    private float cooldownTimer = Mathf.Infinity;
    public bool isBlocking = false;

    //attack mechanic
    public bool isAttacking = false;
    private float attackDuration = 0.25f;
    private float attackTimer = 0f;

    [Header("Mana")]
    [SerializeField] public float StartingMana = 60f;
    [SerializeField] public float ManaCostperCast = 20f;


    public float MaximumMana;
    public float CurrentMana;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        MaximumMana = StartingMana;

        GameSaveData saveData = SaveManager.instance.GetSaveData();

        if (saveData != null)
        {
            MaximumMana = saveData.playerMaximumMana;
            damage = saveData.playerDamage;
        }

        CurrentMana = MaximumMana;
    }

    private void Update()
    {
        //Attack
        if (Input.GetMouseButtonDown(0) && cooldownTimer >= AttackCooldown && playerMovement.canAttack())
        {
            Attack();

            // Set isAttacking to true
            isAttacking = true;
            attackTimer = attackDuration;
        }


        //Jump Attack
        if (Input.GetMouseButtonDown(0) && cooldownTimer >= AttackCooldown && playerMovement.canJumpAttack())
        {
            JumpAttack();

            // Set isAttacking to true
            isAttacking = true;
            attackTimer = attackDuration;
        }

        // Update the attack timer
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                // Clear isAttacking after the attack duration has passed
                isAttacking = false;
            }
        }

        
        //Cast Spell
        if (Input.GetKey(KeyCode.Q) && cooldownTimer >= AttackCooldown && playerMovement.canAttack() && CurrentMana >= ManaCostperCast)
            Cast();

        //Block
        if (Input.GetMouseButton(1) && playerMovement.canBlock() && !isBlocking)
        {
            isBlocking = true;
            Block();
        }
        if (!Input.GetMouseButton(1) && playerMovement.canBlock() && isBlocking)
        {
            isBlocking = false;
            stopBlock();
        }
        cooldownTimer += Time.deltaTime;



    }

    private bool EnemyInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<EnemyHealth>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;


        SoundManager.instance.PlaySound(meleehitSound);
    }

    private void JumpAttack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;


        SoundManager.instance.PlaySound(meleehitSound);
    }


    private void Block()
    {
        anim.SetBool("block", true);
    }

    private void stopBlock()
    {
        anim.SetBool("block", false);
    }

    private void Cast()
    {
        SoundManager.instance.PlaySound(fireballSound);

        anim.SetTrigger("cast");
        cooldownTimer = 0;

        //Cost Mana
        CurrentMana -= ManaCostperCast;

        //pool fireball
        Fireballs[FindFireballs()].transform.position = firePoint.position;
        Fireballs[FindFireballs()].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireballs()
    {
        for(int i = 0; i <Fireballs.Length; i++)
        {
            if (!Fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public void DamageEnemy()
    {
        if (EnemyInAttackRange())
        {
            //Damage enemy health
            enemyHealth.TakeDamage(damage);
            bossHealth.TakeDamge(damage);
        }
    }

}
