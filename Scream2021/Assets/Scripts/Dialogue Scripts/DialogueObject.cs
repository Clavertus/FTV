
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")] 
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] string[] dialogue;

    public string[] Dialogue => dialogue; 
}
