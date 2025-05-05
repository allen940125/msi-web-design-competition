using UnityEngine;

public class Bed : InteractableObject 
{
    [SerializeField] TextAsset dialougeOnBed;
    [SerializeField] TextAsset dialougeUnderBed;
    public void ChickOnBed()
    {
        Debug.Log("Chick On Bed");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBed);
    }
    
    public void ChickUnderBed()
    {
        DialogueManager.Instance.LoadAndStartDialogue(dialougeUnderBed);
        Debug.Log("Chick On Bed");
    }
}