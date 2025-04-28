using UnityEngine;

public class Table : InteractableObject
{
    public GameObject uIInvestigateSafe;

    public void InvestigateSafeOnTable()
    {
        Debug.Log("InvestigateSafeOnTable 打開保險箱");
        uIInvestigateSafe.SetActive(true);
    }


    // 自訂條件檢查方法
    private bool CheckPlayerOnChair() {
        return InteractionManager.Instance.isOnChair; // 假設有個條件A的狀態管理
    }
}