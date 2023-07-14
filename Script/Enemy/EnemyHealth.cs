using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float MaximumHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    private Animator anim;
    private bool dead;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    //The comment part is whats im planing to use for respawning back the enemy after player respawn!!

    /*
    [Header("Reference")]
    [SerializeField] private Health playerHealth;
    */

    private MeleeEnemy meleeEnemy;


    //Experience give on death
    [Header("ExpGive")]
    [SerializeField] private int expAmount = 100;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        meleeEnemy = GetComponent<MeleeEnemy>();
        
        MaximumHealth = startingHealth;
        CurrentHealth = MaximumHealth;

    }
    /*
    private void Update()
    {
        if(dead && playerHealth.isRespawning == true)
        {
            EnemyRespawn();

            Debug.Log("Enemy Respawned!");
        }
    }
    */
    public void TakeDamage(float _damage)
    {
        /*if (meleeEnemy.blockingSucceed)
        {
            return;
        }
        else
        {
        */
        if (meleeEnemy.isBlocking)
        {
            _damage = 0;
        }

        

        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, MaximumHealth);

        if (CurrentHealth > 0)
           {
           if (!meleeEnemy.isBlocking)
           {
                //enemy hurt
                anim.SetTrigger("hurt");

               SoundManager.instance.PlaySound(hurtSound);
           }
           else return;
        }

        else
        {
           if (!dead)
           {
                anim.SetTrigger("die");
                ExperienceManager.Instance.AddExperience(expAmount);
                
                SoundManager.instance.PlaySound(dieSound);

                if (GetComponentInParent<EnemyPatroll>() != null)
                    GetComponentInParent<EnemyPatroll>().enabled = false;
                if (GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;

                dead = true;
                                
           }
        }
        //}
    }

    /*
    public void EnemyRespawn()
    {
        
        dead = false;
        CurrentHealth = MaximumHealth;

        GetComponentInParent<EnemyPatroll>().enabled = true;
        GetComponent<MeleeEnemy>().enabled = true;

        meleeEnemy.Activate();
    }
    */
}
