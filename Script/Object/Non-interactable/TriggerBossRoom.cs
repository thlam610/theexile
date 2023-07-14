using UnityEngine;

public class TriggerBossRoom : MonoBehaviour
{

    [SerializeField] private CameraMoveToNewRoom cameraController;
    [SerializeField] private float bossMinX = 21.5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BossHealth bossHealth;
    private bool hasPlayerPassed = false;
    private Collider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //Reset the trigger if player respawn
        if (playerTransform.position.x < 0.1f || bossHealth.isDead)
        {
            hasPlayerPassed = false;
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayerPassed)
        {
            hasPlayerPassed = true;

            cameraController.SetBossRoomCamera(bossMinX);
                        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasPlayerPassed)
        {
            // Disable the isTrigger property of the collider
            triggerCollider.isTrigger = false;
        }
    }


}
