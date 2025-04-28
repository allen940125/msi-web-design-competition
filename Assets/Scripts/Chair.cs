using UnityEngine;

public class Chair : InteractableObject
{
    [SerializeField] private Transform sitOnPos;
    
    public void SitOnChair()
    {
        GameManager.Instance.Player.transform.position = sitOnPos.position;
        GameManager.Instance.Player.transform.rotation = sitOnPos.rotation;
        
        InteractionManager.Instance.isOnChair = true;
    }

    // void Update()
    // {
    //     if (InteractionManager.Instance.isOnChair)
    //     {
    //         if(Inp)
    //     }
    //
    // }
    //

    // 自訂條件檢查方法
    private bool CheckPlayerNoOnChair() {
        return !InteractionManager.Instance.isOnChair; // 假設有個條件A的狀態管理
    }
}