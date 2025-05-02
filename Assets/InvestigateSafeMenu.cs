using System;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateSafeMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseButtonClicked);
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
}
