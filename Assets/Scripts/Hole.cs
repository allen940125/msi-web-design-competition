using Game.Audio;
using UnityEngine;

public class Hole : InteractableObject
{
    [SerializeField] TextAsset dialougeHole;
    public void ChickHole()
    {
        Debug.Log("Chick Hole");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        DialogueManager.Instance.LoadAndStartDialogue(dialougeHole);
    }
}