using System;
using System.Collections.Generic;
using Game.UI;
using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    
    public class ItemAcquisitionInformationWindow : BasePanel
    {
        public GameObject informationPanel; // UI 面板
        public TMP_Text textItem;
        public Button closeButton;

        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(ClosePanel);
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                // 這邊放你要做的事，例如：
                ClosePanel();
                Debug.Log("有按鍵被按下了！");
            }
        }

        public void SwitchPanel(string textItem) 
        {
            this.textItem.text = textItem;
        }
    }
}