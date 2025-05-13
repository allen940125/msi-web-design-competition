using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateSafeMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [SerializeField] private Button oneNumberButton;
    [SerializeField] private Button twoNumberButton;
    [SerializeField] private Button threeNumberButton;
    [SerializeField] private Button fourNumberButton;
    [SerializeField] private Button fiveNumberButton;
    [SerializeField] private Button sixNumberButton;
    [SerializeField] private Button sevenNumberButton;
    [SerializeField] private Button eightNumberButton;
    [SerializeField] private Button nineNumberButton;
    [SerializeField] private Button zeroNumberButton;
    [SerializeField] private Button enterButton;

    [SerializeField] private TMP_Text enterPassword;
    [SerializeField] private int targetPassword;

    private string currentInput = "";
    private bool isUnlocked = false; // <- 加這個來記錄是否已解鎖

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        oneNumberButton.onClick.AddListener(() => AppendNumber("1"));
        twoNumberButton.onClick.AddListener(() => AppendNumber("2"));
        threeNumberButton.onClick.AddListener(() => AppendNumber("3"));
        fourNumberButton.onClick.AddListener(() => AppendNumber("4"));
        fiveNumberButton.onClick.AddListener(() => AppendNumber("5"));
        sixNumberButton.onClick.AddListener(() => AppendNumber("6"));
        sevenNumberButton.onClick.AddListener(() => AppendNumber("7"));
        eightNumberButton.onClick.AddListener(() => AppendNumber("8"));
        nineNumberButton.onClick.AddListener(() => AppendNumber("9"));
        zeroNumberButton.onClick.AddListener(() => AppendNumber("0"));

        enterButton.onClick.AddListener(OnEnterClicked);
    }

    private void OnEnable()
    {
        ResetInput();
    }

    private void Start()
    {
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier = 0f;
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier = 1.0f;
    }

    private void AppendNumber(string number)
    {
        if (isUnlocked) return; // 已解鎖就不允許輸入
        if (currentInput.Length >= 6) return;

        currentInput += number;
        enterPassword.text = currentInput;
    }

    private void OnEnterClicked()
    {
        if (isUnlocked) return; // 已解鎖就不允許再驗證

        if (int.TryParse(currentInput, out int enteredValue))
        {
            if (enteredValue == targetPassword)
            {
                Debug.Log("密碼正確！保險箱打開。");
                isUnlocked = true; // 設為已解鎖狀態
                OnCloseButtonClicked();
                // TODO: 呼叫開啟保險箱的邏輯
                InventoryManager.Instance.AddItemShowUI(113);
            }
            else
            {
                Debug.Log("密碼錯誤！");
                ResetInput();
            }
        }
        else
        {
            Debug.Log("無效輸入！");
            ResetInput();
        }
    }

    private void ResetInput()
    {
        if (isUnlocked) return; // 解鎖後不再清除輸入或更新 UI

        currentInput = "";
        enterPassword.text = "";
    }
}
