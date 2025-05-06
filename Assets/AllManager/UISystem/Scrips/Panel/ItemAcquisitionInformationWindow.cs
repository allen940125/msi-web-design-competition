using System;
using System.Collections.Generic;
using Game.Audio;
using Game.UI;
using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    
    public class ItemAcquisitionInformationWindow : BasePanel
    {
        [SerializeField] private GameObject informationPanel; // UI 面板
        [SerializeField] private TMP_Text informationText;
        [SerializeField] private Image informationImage;
        [SerializeField] private Button closeButton;

        [Header("音樂")]
        [SerializeField] private AudioData audioData;
        
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlayRandomSFX(audioData);
            closeButton.onClick.AddListener(ClosePanel);
            DialogueManager.Instance.isItemAcquisitionInformationWindowOpen = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueManager.Instance.isItemAcquisitionInformationWindowOpen = false;
        }

        private void Update()
        {
            if (
                UnityEngine.Input.GetKeyDown(KeyCode.W) ||
                UnityEngine.Input.GetKeyDown(KeyCode.A) ||
                UnityEngine.Input.GetKeyDown(KeyCode.S) ||
                UnityEngine.Input.GetKeyDown(KeyCode.D) ||
                UnityEngine.Input.GetKeyDown(KeyCode.Space) ||
                UnityEngine.Input.GetMouseButtonDown(0) // 左鍵
            )
            {
                // 這邊放你要做的事，例如：
                ClosePanel();
                Debug.Log("偵測到指定按鍵被按下！");
            }
        }


        public void SwitchPanel(string textItem) 
        {
            informationText.text = textItem;
        }

        public void SwitchImage(Sprite image)
        {
            informationImage.sprite = image;
        }
    }
}