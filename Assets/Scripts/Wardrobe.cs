using Game.Audio;
using UnityEngine;

public class Wardrobe : InteractableObject
{
    [SerializeField] private int currentTakeId;
    [SerializeField] private int maxTakeId = 4;
    
    [Header("書櫃")]
    [SerializeField] TextAsset dialougeOnWardrobe1;
    [SerializeField] TextAsset dialougeOnWardrobe2;
    [SerializeField] TextAsset dialougeOnWardrobe3;
    [SerializeField] TextAsset dialougeOnWardrobe4;
    
    public void TakeClothingOnWardrobe()
    {
        Debug.Log("TakeClothingOnWardrobe 在衣櫃上拿衣服");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        if (currentTakeId <= maxTakeId)
        {
            currentTakeId++;
        
            if (currentTakeId == 1)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnWardrobe1);
            }
            else if(currentTakeId == 2)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnWardrobe2);
            }
            else if(currentTakeId == 3)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnWardrobe3);
            }
            else if(currentTakeId == 4)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialougeOnWardrobe4);
            }
        }
        else
        {
            DialogueManager.Instance.LoadAndStartDialogue(dialougeOnWardrobe4);
        }
    }
}
