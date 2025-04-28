using UnityEngine;

/// <summary>
/// 對話條件檢查器
/// </summary>
public class DialogueConditionChecker
{
    /// <summary>
    /// 檢查條件
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static bool CheckCondition(string condition)
    {
        //沒有條件的欄位直接過
        if (string.IsNullOrEmpty(condition)) return true;
        
        var parts = condition.Split(':');
        if (parts.Length < 2) return false;

        string type = parts[0];
        string param = parts[1];

        switch (type.ToLower())
        {
            case "item":
                return HandleItemCondition(param);
            
            case "quest":
                return HandleQuestCondition(param);
            
            default:
                Debug.LogError($"未知條件類型: {type}");
                return false;
        }
    }
    
    private static bool HandleItemCondition(string param)
    {
        // 原有物品檢查邏輯...
        var itemParts = param.Split('x');
        int itemId = int.Parse(itemParts[0]);
        int requiredCount = itemParts.Length > 1 ? int.Parse(itemParts[1]) : 1;
            
        var item = InventoryManager.Instance.GetInventoryData(itemId);
        return item != null && item.quantity >= requiredCount;
    }
    
    private static bool HandleQuestCondition(string param)
    {
        var questParams = param.Split('=');
        if (questParams.Length < 2) return false;

        int questID = int.Parse(questParams[0]);
        string requiredStatus = questParams[1].ToLower();

        Debug.Log("任務ID" + questID + "任務目標為" + requiredStatus);
        
        QuestStatus actualStatus = QuestManager.Instance.GetQuestStatus(questID);
        
        return requiredStatus switch
        {
            "notstarted" => actualStatus == QuestStatus.NotStarted,
            "inprogress" => actualStatus == QuestStatus.InProgress,
            "completed"  => actualStatus == QuestStatus.Completed,
            "failed"  => actualStatus == QuestStatus.Failed,
            _ => false
        };
    }
}