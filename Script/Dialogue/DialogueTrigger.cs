using UnityEngine;

public class DialogueTrigger : InteractableObject
{
        
    public Dialogue dialogue;

    public override void Interact()
    {
        // Define the interaction logic for checkpoints
        Debug.Log("Interacting with a NPC!");
        // Perform checkpoint-specific actions

        TriggerDialogue();

        Debug.Log("Finished interact with a NPC!");
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
