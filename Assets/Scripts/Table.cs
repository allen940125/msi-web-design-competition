using Game.Audio;
using UnityEngine;

public class Table : InteractableObject
{
    [Header("小桌子")]
    public GameObject uIInvestigateSafe;
    [SerializeField] private TextAsset dialougeOnTable;
    [SerializeField] private TextAsset dialougeOnTableSafe;

    [Header("大桌子")]
    [SerializeField] private TextAsset dialougeOnBigTable;

    [Header("電腦相關")]
    [SerializeField] private TextAsset _dialogueFileClickOnTableComputer_SwitchComputer;
    [SerializeField] private TextAsset _dialogueFileClickOnTableComputer_NoSwitchComputer;
    [SerializeField] private AudioData _audioClickOnTableComputer_NoSwitchComputer;

    [SerializeField] private TextAsset _dialogueFileClickOnTableComputer_SwitchComputerHaveItem113;
    [SerializeField] private TextAsset _dialogueFileClickOnTableComputer_SwitchComputerNoHaveItem113;

    private bool _isComputerSwitched = false; // <- 新增狀態變數

    public void ChickOnTable()
    {
        Debug.Log("Chick On Table");
        AudioManager.Instance.PlayRandomSFX(base.audioData);
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
        AudioManager.Instance.PlayRandomSFX(base.audioData);
        DialogueManager.Instance.LoadAndStartDialogue(dialougeOnBigTable);
    }

    /// <summary>
    /// 點擊電腦的「播放機密文件」按鈕
    /// </summary>
    public void ClickOnTableComputer()
    {
        Debug.Log("Click On Table Computer");

        if (_isComputerSwitched)
        {
            // ✅ 已經切換過電腦，播放新電腦的對話
            DialogueManager.Instance.LoadAndStartDialogue(_dialogueFileClickOnTableComputer_SwitchComputer);
        }
        else
        {
            // ❌ 尚未切換電腦，播放雜訊與對話
            AudioManager.Instance.PlaySFX(_audioClickOnTableComputer_NoSwitchComputer);
            DialogueManager.Instance.LoadAndStartDialogue(_dialogueFileClickOnTableComputer_NoSwitchComputer);
        }
    }

    /// <summary>
    /// 點擊電腦的「更換電腦」按鈕
    /// </summary>
    public void SwitchOnTableComputer()
    {
        Debug.Log("Switch On Table Computer");

        bool hasNewComputer = InventoryManager.Instance.GetInventoryData(113) != null;

        if (hasNewComputer)
        {
            DialogueManager.Instance.LoadAndStartDialogue(_dialogueFileClickOnTableComputer_SwitchComputerHaveItem113);
            _isComputerSwitched = true; // ✅ 狀態設為「已切換」
        }
        else
        {
            DialogueManager.Instance.LoadAndStartDialogue(_dialogueFileClickOnTableComputer_SwitchComputerNoHaveItem113);
        }
    }

    private bool CheckPlayerOnChair()
    {
        return InteractionManager.Instance.isOnChair;
    }
}
