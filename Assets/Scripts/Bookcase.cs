using System;
using Game.Audio;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class Bookcase : InteractableObject
{
    [SerializeField] private int currentTakeId;
    [SerializeField] private int maxTakeId = 3;
    
    [Header("書櫃")]
    [SerializeField] TextAsset dialougeOnBookcase1;
    [SerializeField] TextAsset dialougeOnBookcase2;
    [SerializeField] TextAsset dialougeOnBookcase3;
    
    public void TakeBookOnBookcase()
    {
        Debug.Log("TakeBookOnBookcase 在書櫃上拿書");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        if (currentTakeId <= maxTakeId)
        {
            currentTakeId++;
        
            if (currentTakeId == 1)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBookcase1);
            }
            else if(currentTakeId == 2)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBookcase2);
            }
            else if(currentTakeId == 3)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBookcase3);
            }
        }
        else
        {
            DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBookcase3);
        }
    }
}
