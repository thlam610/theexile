using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maximumHealth;
    private float currentHealth;

    [Header("ExpGive")]
    [SerializeField] private int expAmount;

    [Header("Audio")]
    [SerializeField] private AudioClip dieSound;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Image healthTotal;
    [SerializeField] private Image healthCurrent;
    [SerializeField] private GameObject bossHealthbar;
    
    public bool isDead { get; private set; }

    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer sr;
    private YetiBoss boss;

    private void Awake()
    {
        currentHealth = maximumHealth;
        isDead = false;

        healthTotal.fillAmount = 1;
        bossHealthbar.SetActive(false);

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        boss = GetComponent<YetiBoss>();

    }

    private void Update()
    {
        if (playerTransform.position.x < 0.1f)
        {
            currentHealth = maximumHealth;
        }

        if (boss.detectPlayer)
        {
            SetUpHealthbar();
        }

        if (isDead)
        {
            bossHealthbar.SetActive(false);
        }
    }

    public void TakeDamge(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, maximumHealth);

        if (currentHealth == 0)
        {
            if (!isDead)
            {
                anim.SetTrigger("die");
                ExperienceManager.Instance.AddExperience(expAmount);

                SoundManager.instance.PlaySound(dieSound);

                boss.enabled = false;

                isDead = true;
            }
        }
    }

    private void Disable()
    {
        boxCollider.enabled = false;
        sr.enabled = false;
    }

    //Set up healthbar
    private void SetUpHealthbar()
    {
        //enable Healthbar
        bossHealthbar.SetActive(true);

        healthCurrent.fillAmount = currentHealth / maximumHealth;
    }
}
