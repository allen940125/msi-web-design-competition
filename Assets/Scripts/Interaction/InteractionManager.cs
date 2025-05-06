using System;
using System.Linq;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
    public float interactionRadius = 1.5f;
    public LayerMask interactionLayer;
    public float detectionAngle = 45f; // 前方檢測範圍角度 (左右各 45°)
    
    private InteractableObject detectedObject;
    private InteractableObject currentObject; // 當前顯示 UI 的物件
    public InteractionUI interactionUI; // 連接 UI 管理腳本
    
    public bool isOnChair = false;
    
    public static InteractionManager Instance {private set; get;}
    void Awake()
    {
        // 確保只有一個實例存在
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 場景切換時不銷毀
        }
        else
        {
            Destroy(gameObject); // 已經有了，就砍掉自己
        }
    }
    
    void Update()
    {
        HandleInteractionInput();
        
        // 持續檢查當前物件是否還在範圍內
        if (currentObject != null) {
            Collider2D col = currentObject.GetComponent<Collider2D>();
            if (col != null) {
                // 使用 Collider 的 ClosestPoint 計算玩家與物件最近距離
                float distance = Vector2.Distance(transform.position, col.ClosestPoint(transform.position));
                if (distance > interactionRadius) {
                    currentObject = null;
                    interactionUI.HideOptions();
                }
            }
        }
    }

    /// <summary>
    /// 偵測可互動物件
    /// </summary>
    void DetectInteractableObjects() {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactionLayer);
    
        detectedObject = hitColliders
            .Select(c => c.GetComponent<InteractableObject>())
            .Where(obj => obj != null)
            // 添加安全检查
            .Where(obj => {
                // 如果全局条件列表未初始化或为空，直接通过
                if (obj.globalConditions == null || obj.globalConditions.Count == 0)
                {
                    return true;
                }

                bool globalPass = true;
                try {
                    globalPass = obj.CheckGlobalConditions(out _);
                } catch (System.Exception e) {
                    Debug.LogError($"全局条件检查出错: {e}");
                    return false;
                }
    
                bool hasAvailableOptions = obj.interactionOptions != null && 
                                           obj.interactionOptions.Any(opt => 
                                               opt != null && 
                                               opt.ShouldShow() && 
                                               obj.CheckConditions(opt,out string failMessage)
                                           );
    
                return globalPass && hasAvailableOptions;
            })
            // 原有角度和距离检查...
            .FirstOrDefault();

        // 简化处理逻辑
        if (detectedObject != null) 
        {
            // 直接检查是否有可立即触发的有效选项
            var immediateOption = detectedObject.interactionOptions?
                .FirstOrDefault(opt => opt != null &&
                                       opt.isImmediate &&
                                       opt.ShouldShow() &&
                                       detectedObject.CheckConditions(opt, out string failMessage));
        
            if (immediateOption != null) 
            {
                detectedObject.Interact(detectedObject.interactionOptions.IndexOf(immediateOption));
            }
            else 
            {
                interactionUI.ShowOptions(detectedObject);
            }
        }
        else 
        {
            interactionUI.HideOptions();
        }

        currentObject = detectedObject;
    }

    /// <summary>
    /// 處理互動輸入
    /// </summary>
    void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (!GameManager.Instance.UIManager.GetPanel<DialogueWindow>(UIType.DialogueWindow))
            {
                if (DialogueManager.Instance.isItemAcquisitionInformationWindowOpen)
                {
                    return;
                }
                DetectInteractableObjects();
            }
        }
    }

    public void SwitchNpcStatus(NpcStatus npcStatus)
    {
        currentObject.currentNpcStatus = npcStatus;
    }
}
