using UnityEngine;


public class Checkpoint : InteractableObject
{
    [SerializeField] private Health playerHealth;


    public override void Interact()
    {
        // Define the interaction logic for checkpoints
        Debug.Log("Interacting with a checkpoint!");
        // Perform checkpoint-specific actions

        playerHealth.Respawn();

        Debug.Log("Finished interact with a checkpoint!");
    }
}
