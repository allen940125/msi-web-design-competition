using Game.Audio;
using UnityEngine;

public class Bed : InteractableObject 
{
    [SerializeField] TextAsset dialougeOnBed;
    [SerializeField] TextAsset dialougeUnderBed;
    public void ChickOnBed()
    {
        Debug.Log("Chick On Bed");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBed);
    }
    
    public void ChickUnderBed()
    {
        DialogueManager.Instance.LoadAndStartDialogue(dialougeUnderBed);
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        Debug.Log("Chick On Bed");
    }
}