using Game.Audio;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] TextAsset dialougeDoor;
    public void ChickDoor()
    {
        Debug.Log("Chick Door");
        //AudioManager.Instance.PlayRandomSFX(base.audioData);
        DialogueManager.Instance.LoadAndStartDialogue(dialougeDoor);
    }
}