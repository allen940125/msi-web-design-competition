using Game.Audio;
using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Game.UI
{
    public class BagMenu : BasePanel
    {
        [Header("通用操作按鈕")]
        [SerializeField] private Button useButton;
        [SerializeField] private Button closeButton;
    
        [Header("物品分類標籤頁")]
        [SerializeField] private Button equipmentTabButton; 
        [SerializeField] private Button consumableTabButton; 
        [SerializeField] private Button materialTabButton;
        [SerializeField] private Button keyItemTabButton;

        [Header("商店資訊")]
        [SerializeField] GameObject prefabSlotStoreItem;
        [SerializeField] GameObject scrollViewContentStoreItemListGrid;
        
        [Header("當前選中物品資訊")]
        [SerializeField] private Image selectedItemIcon;
        [SerializeField] private TMP_Text selectedItemName;
        [SerializeField] private TMP_Text selectedItemDescription;

        protected override void Awake()
        {
            base.Awake();
            
            UIFontManager.Instance.ApplyFont(selectedItemName);
            UIFontManager.Instance.ApplyFont(selectedItemDescription);

            GameManager.Instance.UIManager.ClosePanel(UIType.GameHUD);
           
            //設定通用按鈕
            InitializeCommonButtons();
            //設定類別按鈕
            InitializeCategoryButtons();
            
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnInventoryItemClickedEvent, OnInventoryItemClickedEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GameManager.Instance.UIManager.OpenPanel<GameHUD>(UIType.GameHUD);
            
            // 取消訂閱事件，避免記憶體洩漏
            GameManager.Instance.MainGameEvent.Unsubscribe<InventoryItemClickedEvent>();
        }
        
        protected override void Start()
        {
            base.Start();

            InventoryManager.InventoryPanelController.SetBagInfo(uiPanel, scrollViewContentStoreItemListGrid, prefabSlotStoreItem);
            
            GameManager.Instance.MainGameEvent.Send(new CursorToggledEvent() { ShowCursor = true });
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Consumable });
        }

        #region 事件訂閱

        private void OnInventoryItemClickedEvent(InventoryItemClickedEvent cmd)
        {
            UpdateClickItemInfo(cmd.StoredInventoryItemRuntimeData);
        }
        
        #endregion
        
        /// <summary>
        /// 更新點擊的物品資訊
        /// </summary>
        /// <param name="inventoryItemRuntimeData"></param>
        void UpdateClickItemInfo(InventoryItemRuntimeData inventoryItemRuntimeData)
        {
            if (inventoryItemRuntimeData == null)
            {
                return;
            }
            selectedItemIcon.color = new Color(255, 255, 255, 255f);
            selectedItemIcon.sprite = inventoryItemRuntimeData.BaseTemplete.ItemIconPath;

            selectedItemName.text = inventoryItemRuntimeData.BaseTemplete.Name;
            selectedItemDescription.text = inventoryItemRuntimeData.BaseTemplete.ItemDescription;
        }
        
        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            useButton.onClick.AddListener(OnUseButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        /// <summary>
        /// 初始化設定類別按鈕
        /// </summary>
        void InitializeCategoryButtons()
        {
            equipmentTabButton.onClick.AddListener(OnEquipmentTabButtonClicked);
            consumableTabButton.onClick.AddListener(OnConsumableTabButtonClicked);
            materialTabButton.onClick.AddListener(OnMaterialTabButtonClicked);
            keyItemTabButton.onClick.AddListener(OnKeyItemTabButtonClicked);
        }

        //設定通用按鈕
        void OnUseButtonClicked()
        {
            if (InventoryManager.Instance.curClickInventoryItemRuntimeData != null)
            {
                if (InventoryManager.Instance.GetInventoryData(105) != null)
                {
                    if (InventoryManager.Instance.GetInventoryData(114) != null)
                    {
                        Debug.Log("這下能聽清楚了吧？讓我聽聽這裏面都是些什麼。");
                        DialogueManager.Instance.LoadAndStartDialogue(DialogueManager.Instance._dialogueFileAsItem105_Have114);
                        AudioManager.Instance.PlaySFX(DialogueManager.Instance._audioAsItem105_Have114);
                        return;
                    }
                    DialogueManager.Instance.LoadAndStartDialogue(DialogueManager.Instance._dialogueFileAsItem105_NoHave114);
                    AudioManager.Instance.PlaySFX(DialogueManager.Instance._audioAsItem105_NoHave114);
                    Debug.Log("吵死了，什麼都聽不到，這人怎麼沒有一個東西是好的阿。看來得找到一個好\n一點的耳機才能聽了。");
                }
                
            }
        }

        void OnCloseButtonClicked()
        {
            Debug.Log("Click OnCloseButtonClicked");
            ClosePanel();
        }

        //設定類別按鈕
        void OnEquipmentTabButtonClicked()
        {
            Debug.Log("Click OnEquipmentTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Equipment});  
        }

        void OnConsumableTabButtonClicked()
        {
            Debug.Log("Click OnConsumableTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Consumable});  
        }
        
        void OnMaterialTabButtonClicked()
        {
            Debug.Log("Click OnMaterialTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Material});  
        }

        void OnKeyItemTabButtonClicked()
        {
            Debug.Log("Click OnKeyItemTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.KeyItem});  
        }
    }

}