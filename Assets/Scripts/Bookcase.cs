using System;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class Bookcase : InteractableObject
{
    [SerializeField] private int currentTakeId;
    [SerializeField] private int maxTakeId = 3;
    
    public void TakeBookOnBookcase()
    {
        Debug.Log("TakeBookOnBookcase 在書櫃上拿書");
        if (currentTakeId <= maxTakeId - 1)
        {
            currentTakeId++;

            if (currentTakeId == 1)
            {
                GetItem(107);
            }
            else if(currentTakeId == 2)
            {
                GetItem(108);
            }
            else if(currentTakeId == 3)
            {
                GetItem(109);
            }
        }
        else
        {
            GetItem(0);
        }
    }

    async void GetItem(int id)
    {
        ItemAcquisitionInformationWindow panel;

        if (id == 0)
        {
            panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
            panel.SwitchPanel("這裡什麼都沒有了。");
            return;
        }
        
        // 獲取物品邏輯
        try
        {
            GameManager.Instance.MainGameEvent.Send(new ItemAddedToBagEvent
            {
                ItemID = id,
                Quantity = 1
            });
            
            var itemRuntimeData = InventoryManager.Instance.GetInventoryData(id);
            
            if (itemRuntimeData == null)
            {
                panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
                panel.SwitchPanel("找不到指定物品的資料！");
                return;
            }
            
            string text = $"<b>獲得：</b> {itemRuntimeData.BaseTemplete.Name}\n<color=#cccccc>物品介紹：</color>\n{itemRuntimeData.BaseTemplete.ItemDescription}";
        
            panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
            panel.SwitchPanel(text);
        }
        catch (Exception e)
        {
            panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
            panel.SwitchPanel("獲取物品時發生未知錯誤！");
            Debug.LogError(e);
        }
    }
}
