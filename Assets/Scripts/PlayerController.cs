using Game.Audio;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    // 假設你會從其他地方控制這些狀態
    public bool isMoving = false; 
    public bool isFinding = false; // 可由其他系統設定
    public bool isSitDown = false; // 是否觸發坐下

    [Header("音效")]
    [SerializeField] AudioData moveAudioData;
    
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        // 取得水平與垂直輸入
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // 設定 Animator 參數
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        // 判斷是否正在移動
        isMoving = movement.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // 設定 isFinding 狀態（你也可以在別的地方動態改變）
        animator.SetBool("isFinding", isFinding);

        // 判斷是否坐下（觸發器只會執行一次）
        animator.SetBool("isSitDown", isSitDown);
    }

    void FixedUpdate() 
    {
        Debug.Log(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier);
        rb.linearVelocity = movement * (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier * moveSpeed);
    }
}