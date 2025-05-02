using UnityEngine;

public class Chair : InteractableObject
{
    [SerializeField] private Transform sitOnPos;
    
    [SerializeField] TextAsset dialougeChair;
    [SerializeField] TextAsset dialougeSitOnChair;
    public void ChickChair()
    {
        Debug.Log("Chick Chair");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeChair);
    }
    
    public void SitOnChair()
    {
        Debug.Log("Chick Sit On Chair");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeSitOnChair);
        
        GameManager.Instance.Player.transform.position = sitOnPos.position;
        GameManager.Instance.Player.transform.rotation = sitOnPos.rotation;
        
        InteractionManager.Instance.isOnChair = true;
    }

    // 自訂條件檢查方法
    private bool CheckPlayerNoOnChair() {
        return !InteractionManager.Instance.isOnChair; // 假設有個條件A的狀態管理
    }
}