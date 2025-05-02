using UnityEngine;

public class GarbageCan : InteractableObject
{
    [SerializeField] TextAsset dialougeGarbageCan;
    public void ChickGarbageCan()
    {
        Debug.Log("Chick GarbageCan");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeGarbageCan);
    }
}
