using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;

    //only read
    public string[] Dialogue => dialogue;
}
