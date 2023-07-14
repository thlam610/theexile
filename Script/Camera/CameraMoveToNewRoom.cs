using UnityEngine;

public class CameraMoveToNewRoom : MonoBehaviour
{
    //Boss Room camera
    [SerializeField] private float bossRoomMinX;


    //Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance = 3f;
    [SerializeField] private float cameraSpeed =1f;
    [SerializeField] private float defaultMinX = 0f;
    [SerializeField] private float maxX = 35f;

    //Boss
    [SerializeField] private BossHealth bossHealth;

    private float lookAhead;
    private float minX;

    private void Awake()
    {
        minX = defaultMinX;
    }

    private void Update()
    {
        //If player respawn unlock the camera
        if (player.position.x < 0.1f || bossHealth.isDead)
        {
            minX = defaultMinX;
        }

        //Follow player
        float targetX = player.position.x + lookAhead;
        float clampedX = Mathf.Clamp(targetX, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, cameraSpeed * Time.deltaTime);
    }

    //Lock camera to prevent player go out of boss room
    public void SetBossRoomCamera(float bossMinX)
    {
        minX = bossMinX;
    }

    //Unlock camera if player want to return to previous room
    public void SetDefaultCamera()
    {
        minX = defaultMinX;
    }
}
