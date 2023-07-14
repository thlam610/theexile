using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Animator anim;

    [Header("PlayerBehaviour")]
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerMovement playerMovement;

    private Queue<string> sentences;
    private bool isOpen = false;
    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if ((isOpen && Input.GetKeyDown(KeyCode.KeypadEnter)) || (isOpen && Input.GetMouseButtonDown(0)))
            DisPlayNextSentence();

        if (isOpen)
        {
            playerAttack.enabled = false;
            playerMovement.enabled = false;
        }
        else if (!isOpen)
        {
            playerAttack.enabled = true;
            playerMovement.enabled = true;
        }

    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("isOpen", true);
        isOpen = true;

        //Debug.Log("Starting conversation with " + dialogue.name);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisPlayNextSentence();
    }

    public void DisPlayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        anim.SetBool("isOpen", false);
        isOpen = false;

        //Debug.Log("End of conversation!");
    }
}
