using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;


    //Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    private float lookAhead;

    private void Update()
    {
        
        //Follow player
        float targetX = player.position.x + lookAhead;
        float clampedX = Mathf.Clamp(targetX, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, cameraSpeed * Time.deltaTime);
    }

}
