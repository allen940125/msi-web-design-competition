using System;
using System.Collections.Generic;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    #region 是否可互動
    
    [Header("全域互動條件")]
    public List<InteractionCondition> globalConditions = new List<InteractionCondition>(); // 新增全域條件

    // InteractableObject.cs 修改后的检查方法
    public bool CheckGlobalConditions(out string failMessage) 
    {
        failMessage = "";
    
        // 处理全局条件列表为空或未初始化的情况
        if (globalConditions == null || globalConditions.Count == 0)
        {
            return true;
        }

        foreach (var condition in globalConditions)
        {
            // 跳过列表中的空元素
            if (condition == null) continue;

            try 
            {
                bool result = condition.conditionType switch
                {
                    InteractionCondition.ConditionType.HasItem => 
                        CheckHasItem(condition.requiredItemID),
                    InteractionCondition.ConditionType.CustomCheck => 
                        (bool)(GetMethodSafely(condition.customMethodName)?.Invoke(this, null) ?? false),
                    _ => true
                };

                if (!result)
                {
                    failMessage = string.IsNullOrEmpty(condition.failPrompt) 
                        ? "条件不满足" 
                        : condition.failPrompt;
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"全局条件检查出错: {e}");
                failMessage = "系统错误";
                return false;
            }
        }
        return true;
    }

// 添加安全反射方法
    private System.Reflection.MethodInfo GetMethodSafely(string methodName)
    {
        if (string.IsNullOrEmpty(methodName)) return null;
    
        try 
        {
            return GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.NonPublic);
        }
        catch 
        {
            return null;
        }
    }
    
    #endregion
    
    [System.Serializable]
    public class InteractionCondition {
        public enum ConditionType {
            HasItem,      // 需要持有特定物品
            CustomCheck   // 自訂檢查方法
        }

        public ConditionType conditionType;
        public int requiredItemID;      // 對應 HasItem
        public string customMethodName; // 對應 CustomCheck
        public string failPrompt;       // 條件失敗時的提示文字
    }
    
    [System.Serializable]
    public class InteractionOption
    {
        public string optionName;
        public bool canRepeat = false;   // 保留此字段
        public bool isImmediate = false;
        public int itemID;
        public string customMethodName;
        public string emptyPrompt = "這裡什麼都沒有了。";
        public List<InteractionCondition> conditions = new List<InteractionCondition>();
    
        [Header("觀察用")]
        public bool hasInteracted = false;
        [NonSerialized] public Action onInteract;

        // 新增屬性：檢查是否可顯示
        public bool ShouldShow() {
            return canRepeat || !hasInteracted;
        }
    }

    public List<InteractionOption> interactionOptions = new List<InteractionOption>();

    protected virtual void Start()
    {
        foreach (var option in interactionOptions)
        {
            if (!string.IsNullOrEmpty(option.customMethodName))
            {
                option.onInteract = () =>
                {
                    var methodInfo = GetType().GetMethod(option.customMethodName,
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

                    if (methodInfo != null)
                    {
                        var parameters = methodInfo.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(InteractionOption))
                        {
                            methodInfo.Invoke(this, new object[] { option });
                        }
                        else if (parameters.Length == 0)
                        {
                            methodInfo.Invoke(this, null);
                        }
                        else
                        {
                            Debug.LogError($"Method {option.customMethodName} has unsupported parameters.");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Cannot find method: {option.customMethodName}");
                    }
                };
            }
            else
            {
                option.onInteract = () => AcquireItem(option);
            }
        }
    }

    public void Interact(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < interactionOptions.Count)
        {
            var option = interactionOptions[optionIndex];
            option.onInteract?.Invoke();
        }
    }

    // 修改後的 AcquireItem 方法
    protected virtual async void AcquireItem(InteractionOption option)
    {
        ItemAcquisitionInformationWindow panel;

        // 檢查是否已互動過且不可重複
        if (option.hasInteracted && !option.canRepeat)
        {
            panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
            panel.SwitchPanel(option.emptyPrompt);
            return;
        }

        // 條件檢查（優先於不可重複提示）
        if (!CheckConditions(option, out string failMessage))
        {
            panel = await GameManager.Instance.UIManager.OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
            panel.SwitchPanel(failMessage);
            return;
        }

        // 檢查是否已互動過
        if (option.hasInteracted || option.itemID == 0)
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
                ItemID = option.itemID,
                Quantity = 1
            });
            
            var itemRuntimeData = InventoryManager.Instance.GetInventoryData(option.itemID);
            
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

        // 更新互動狀態
        if (!option.canRepeat)
        {
            option.hasInteracted = true;
        }
    }

// 在 InteractableObject 中添加物品檢查基礎方法
    protected bool CheckHasItem(int itemID) {
        // 假設你的物品系統存在於 GameManager 中
        return InventoryManager.Instance.GetInventoryData(itemID) != null;
    }

// 修改 CheckConditions 方法
    protected bool CheckConditions(InteractionOption option, out string failMessage) {
        foreach (var condition in option.conditions) {
            bool result = condition.conditionType switch {
                InteractionCondition.ConditionType.HasItem => 
                    CheckHasItem(condition.requiredItemID),
                InteractionCondition.ConditionType.CustomCheck => 
                    (bool)GetType().GetMethod(condition.customMethodName)?
                        .Invoke(this, null),
                _ => true
            };
        
            if (!result) {
                failMessage = condition.failPrompt; // 使用 InteractionCondition 的提示
                return false;
            }
        }
        failMessage = "";
        return true;
    }

// 新增失敗提示方法
    private async void ShowFailPrompt(string message) {
        var panel = await GameManager.Instance.UIManager
            .OpenPanel<ItemAcquisitionInformationWindow>(UIType.ItemAcquisitionInformationWindow);
        panel.SwitchPanel(message);
    }
    
    public bool CheckConditionsMet(InteractionOption option) {
        foreach (var condition in option.conditions) {
            bool result = condition.conditionType switch {
                InteractionCondition.ConditionType.HasItem => CheckHasItem(condition.requiredItemID),
                InteractionCondition.ConditionType.CustomCheck => (bool)GetType().GetMethod(condition.customMethodName)?
                    .Invoke(this, null),
                _ => true
            };
            if (!result) return false;
        }
        return true;
    }
}