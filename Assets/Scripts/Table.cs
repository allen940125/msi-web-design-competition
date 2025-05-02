using UnityEngine;

public class Table : InteractableObject
{
    [Header("小桌子")]
    public GameObject uIInvestigateSafe;
    [SerializeField] TextAsset dialougeOnTable;
    [SerializeField] TextAsset dialougeOnTableSafe;
    
    [Header("大桌子")]
    [SerializeField] TextAsset dialougeOnBigTable;

    public void ChickOnTable()
    {
        Debug.Log("Chick On Table");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnTable);
    }
    
    public void ChickSafeOnTable()
    {
        Debug.Log("Chick Safe On Table 打開保險箱");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnTableSafe);
        uIInvestigateSafe.SetActive(true);
    }

    public void ChickOnBigTable()
    {
        Debug.Log("Chick On BigTable");
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBigTable);
    }
    
    // 自訂條件檢查方法
    private bool CheckPlayerOnChair() {
        return InteractionManager.Instance.isOnChair; // 假設有個條件A的狀態管理
    }
}