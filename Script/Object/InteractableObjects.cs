using UnityEngine;


public class InteractableObjects : MonoBehaviour
{
    [SerializeField] private Transform interactSignal;
    private InteractableObject interactableObject;
    private bool isInRange;

    private void Awake()
    {
        interactSignal.gameObject.SetActive(false);
        isInRange = false;

        // Get the appropriate subclass based on the type of Interactable Object
        interactableObject = GetComponentInChildren<InteractableObject>(true);
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactableObject.Interact(); // Call the appropriate subclass's Interact method
            interactSignal.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactSignal.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            interactSignal.gameObject.SetActive(false);
        }
    }


}
