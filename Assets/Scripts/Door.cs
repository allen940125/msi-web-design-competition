using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] TextAsset dialougeDoor;
    public void ChickDoor()
    {
        Debug.Log("Chick Door");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeDoor);
    }
}