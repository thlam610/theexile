using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] public float spellDamage = 300f;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator anim;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 3)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        if (collision.tag == "Enemy")
            collision.GetComponent<EnemyHealth>().TakeDamage(spellDamage);

        if (collision.tag == "Boss")
            collision.GetComponent<BossHealth>().TakeDamge(spellDamage);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localscaleX = transform.localScale.x;
        if (Mathf.Sign(localscaleX) != _direction)
            localscaleX = -localscaleX;

        transform.localScale = new Vector3(localscaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
