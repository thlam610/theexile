using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private UIManager uiManager;
    public Transform currentCheckpoint;
    //we will store our last checkpoint here
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();

        /*
        GameSaveData saveData = SaveManager.instance.GetSaveData();

        if (saveData != null)
        {
            currentCheckpoint.position = saveData.playerCheckpointPosition;
        }
        */
    }

    public void CheckRespawn()
    {

        uiManager.GameOver();


        transform.position = new Vector3(currentCheckpoint.position.x, transform.position.y, transform.position.z); //Move player to checkpoint position
                                                                                                                        //Restore player's health and reset animation
        playerHealth.Respawn();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Store the checkpoint that we activated as the current one
            Debug.Log("claimed checkpoint");

            SaveManager.instance.Save();
        }
    }
}
