using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueTextLabel;
    [SerializeField] public DialogueObject testDialogue;
    [SerializeField] private GameObject dialogueBox;

    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        //Show dialogue box at the start of game 
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(1);

        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, dialogueTextLabel);

            //wait for input 
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        }
        
        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        dialogueTextLabel.text = string.Empty;
    }
}
