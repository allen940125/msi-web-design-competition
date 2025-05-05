using UnityEngine;

public class Hole : InteractableObject
{
    [SerializeField] TextAsset dialougeHole;
    public void ChickHole()
    {
        Debug.Log("Chick Hole");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeHole);
    }
}