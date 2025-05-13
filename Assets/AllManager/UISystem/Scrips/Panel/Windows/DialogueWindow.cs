using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : BasePanel
{
    [SerializeField] Image characterImage;
    
    [SerializeField] Sprite playerSprite;
    [SerializeField] Sprite narrationSprite;

    
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text dialogueBodyText;
    [SerializeField] GameObject optionBtnPrefab;
    [SerializeField] Transform optionBtnContainer;
    
    private void OnEnable()
    {
        var dm = DialogueManager.Instance;
        dm.OnLineChanged      += HandleLine;
        dm.OnOptionsAvailable += ShowOptions;
        dm.OnDialogueEnd      += ClosePanel;
        dm.OnStoreOpened      += OpenStore;
    }

    private void OnDisable()
    {
        var dm = DialogueManager.Instance;
        dm.OnLineChanged      -= HandleLine;
        dm.OnOptionsAvailable -= ShowOptions;
        dm.OnDialogueEnd      -= ClosePanel;
        dm.OnStoreOpened      -= OpenStore;
        
        GameManager.Instance.Player.GetComponent<PlayerController>().isFinding = false;
    }

    private void Update()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().isFinding = true;
    }

    private void HandleLine(DialogueLine line)
    {
        switch (line.CharacterName)
        {
            case "玩家":
                characterImage.color = new Color(255, 255, 255, 255);
                characterImage.sprite = playerSprite;
                break;
            case "旁白":
                characterImage.color = new Color(255, 255, 255, 0);
                break;
            default:
                characterImage.color = new Color(255, 255, 255, 255);
                characterImage.sprite = playerSprite; // 或者放一個預設圖
                break;
        }

        characterNameText.text = line.CharacterName;
        dialogueBodyText.text = line.Text;
        ClearOptions();
    }


    private void ShowOptions(List<DialogueLine> opts)
    {
        ClearOptions();
        var validOptions = opts.Where(opt => 
            DialogueConditionChecker.CheckCondition(opt.Condition)
        ).ToList();

        for (int i = 0; i < validOptions.Count; i++)
        {
            var opt = validOptions[i];
            var btn = Instantiate(optionBtnPrefab, optionBtnContainer);
            btn.GetComponentInChildren<TMP_Text>().text = opt.Text;

            int idx = i;
            btn.GetComponent<Button>().onClick.AddListener(() => 
                DialogueManager.Instance.SelectOption(idx)
            );
        }
    }
    
    private void ClearOptions()
    {
        foreach (Transform t in optionBtnContainer)
            Destroy(t.gameObject);
    }

    private void OpenStore()
    {
        // 打开商店 UI
        ClosePanel();
        
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier = 1f;
    }
}