using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : InteractableObject
{
    public override void Interact()
    {
        // Define the interaction logic for checkpoints
        Debug.Log("Interacting with a gate!");

        // Perform gate action
        SceneManager.LoadScene(2);        
    }
}
