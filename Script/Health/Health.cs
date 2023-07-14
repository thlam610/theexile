using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float MaximumHealth;
    public float CurrentHealth;
    private Animator anim;
    private bool dead;

    
    [Header("iFrame")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int NumberOfFlashes = 3;
    private SpriteRenderer sr;
    

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip blockSound;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private PlayerAttack playerAttack;
    private ComponentResetter componentResetter;
    private bool invulnerable;
    public bool isRespawning { get; private set; }

    private void Awake()
    {
        MaximumHealth = startingHealth;
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        componentResetter = GetComponent<ComponentResetter>();


        //Load the save data from Save Manager
        GameSaveData saveData = SaveManager.instance.GetSaveData();

        if (saveData != null)
        {
            // Update the player's stats based on the save data
            MaximumHealth = saveData.playerMaximumHealth;
        }

        CurrentHealth = MaximumHealth;
    }

    private void Update()
    {
        if (isRespawning)
        {
            if (Input.anyKeyDown)
            {
                isRespawning = false;
            }            
        }
    }

    public void TakeDamage(float _damage)
    {
        if (playerAttack.isBlocking)
        {
            _damage = _damage * 0.5f;
            SoundManager.instance.PlaySound(blockSound);
        }

        if (invulnerable) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, MaximumHealth);
        
        if (CurrentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hurtSound);

            //iframes
            //StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                //player cook
                anim.SetTrigger("die");
                SoundManager.instance.PlaySound(dieSound);

                //Deactivate all attached componenets
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
                
            }

        }
        
    }

    public void AddHealth(float _healAmount)
    {
       CurrentHealth = Mathf.Clamp(CurrentHealth + _healAmount, 0, MaximumHealth);
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8,9, true);
        //iFrames duration
        for (int i = 0; i < NumberOfFlashes; i++)
        {
            sr.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (NumberOfFlashes *2));
            sr.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (NumberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;
    }

    public void Respawn()
    {
        dead = false;
        isRespawning = true;

        AddHealth(MaximumHealth);
        playerAttack.CurrentMana = playerAttack.MaximumMana;
        anim.ResetTrigger("die");
        anim.Play("Idle");

        
        foreach (Behaviour component in components)
            component.enabled = true;

        
    }
}
