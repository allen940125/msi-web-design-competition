using Game.Audio;
using UnityEngine;

public class GarbageCan : InteractableObject
{
    [SerializeField] TextAsset dialougeGarbageCan;
    public void ChickGarbageCan()
    {
        Debug.Log("Chick GarbageCan");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        DialogueManager.Instance.LoadAndStartDialogue(dialougeGarbageCan);
    }
}
