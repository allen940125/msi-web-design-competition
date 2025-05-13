using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThisGameMainMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        StartCoroutine(DelayCloseMenu());
    }

    private IEnumerator DelayCloseMenu()
    {
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier = 1.0f;
        closeButton.GetComponent<Image>().color = Color.clear; // 等一切都做完再關閉自己

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GamePlaying = true;
        gameObject.SetActive(false);
    }

}