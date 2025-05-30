using Game.UI;
using UnityEngine;

public class DialogueEffectExecutor
{
    public static void ExecuteEffect(string effect)
    {
        if (string.IsNullOrEmpty(effect)) return;

        var commands = effect.Split('|');
        foreach (var cmd in commands)
        {
            var parts = cmd.Split(':');
            if (parts.Length < 2) continue;

            string type = parts[0].Trim().ToLower();
            string param = parts[1].Trim();

            switch (type)
            {
                case "give":
                    HandleItemGiveEffect(param);
                    break;
                case "quest":
                    HandleQuestEffect(param);
                    break;
                case "npc":
                    HandleNpcEffect(param);
                    break;
                // 可擴充其他效果...
            }
        }
    }

    private static void HandleItemGiveEffect(string param)
    {
        var itemData = param.Split('x');
        int itemId = int.Parse(itemData[0]);
        int quantity = itemData.Length > 1 ? int.Parse(itemData[1]) : 1;
        InventoryManager.Instance.AddItemShowUI(itemId, quantity);
        Debug.Log("Giving " + itemId + " to " + quantity);
    }
    
    private static void HandleQuestEffect(string param)
    {
        var questParams = param.Split('=');
        if (questParams.Length < 2) return;

        int questID = int.Parse(questParams[0]);
        string action = questParams[1].ToLower();

        switch (action)
        {
            case "inprogress":
                QuestManager.Instance.StartQuest(questID);
                break;
            case "completed":
                QuestManager.Instance.CompleteQuest(questID);
                break;
            case "failed":
                QuestManager.Instance.FailedQuest(questID);
                break;
        }
    }
    
    private static void HandleNpcEffect(string param)
    {
        switch (param)
        {
            case "inprogress":
                InteractionManager.Instance.SwitchNpcStatus(NpcStatus.InProgress);
                break;
            case "completed":
                InteractionManager.Instance.SwitchNpcStatus(NpcStatus.Completed);
                break;
            case "failed":
                InteractionManager.Instance.SwitchNpcStatus(NpcStatus.Failed);
                break;
        }
    }
}
