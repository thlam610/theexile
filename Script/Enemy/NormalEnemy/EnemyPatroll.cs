using UnityEngine;

public class EnemyPatroll : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform Enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("IdleState")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = Enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (Enemy.position.x >= leftEdge.position.x)
            MoveInDirection(-1);
            else
            {
                //change direction
                DirectionChange();
            }
        }
        else
        {
            if (Enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                //change direction
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        //Make enemy face direction
        Enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        //Move in that direction
        Enemy.position = new Vector3(Enemy.position.x + Time.deltaTime * _direction * speed, Enemy.position.y, Enemy.position.z);

    }
}
