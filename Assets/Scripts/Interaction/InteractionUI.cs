using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour {
    public GameObject optionPanel; // UI 面板
    public Button optionButtonPrefab; // 選項按鈕預置物
    private InteractableObject currentObject;
    private List<Button> buttons = new List<Button>();
    private int selectedIndex = 0;

    public void ShowOptions(InteractableObject obj) {
        currentObject = obj;
        ClearOptions();

        GameManager.Instance.Player.GetComponent<PlayerController>().isFinding = true;
        
        // 檢查是否有需要直接觸發的選項
        var immediateOption = obj.interactionOptions.FirstOrDefault(opt => opt.isImmediate);
        if (immediateOption != null) {
            // 直接觸發第一個可立即執行的選項
            obj.Interact(obj.interactionOptions.IndexOf(immediateOption));
            return; // 不顯示按鈕
        }

        // 正常顯示按鈕邏輯
        for (int i = 0; i < obj.interactionOptions.Count; i++) {
            int index = i;
            Button newButton = Instantiate(optionButtonPrefab, optionPanel.transform);
            newButton.GetComponentInChildren<TMP_Text>().text = obj.interactionOptions[i].optionName;
            newButton.onClick.AddListener(() => obj.Interact(index));
            newButton.onClick.AddListener(InteractionManager.Instance.interactionUI.HideOptions);
            buttons.Add(newButton);
        }

        optionPanel.SetActive(true);
    }

    public void HideOptions() {
        optionPanel.SetActive(false);
        GameManager.Instance.Player.GetComponent<PlayerController>().isFinding = false;
        currentObject = null;
    }

    private void ClearOptions() {
        foreach (var btn in buttons) Destroy(btn.gameObject);
        GameManager.Instance.Player.GetComponent<PlayerController>().isFinding = false;
        buttons.Clear();
    }
}