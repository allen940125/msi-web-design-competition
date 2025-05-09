using Game.Audio;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    public bool canMove = true; // 🔹 當為 false 時無法移動

    public bool isMoving = false; 
    public bool isFinding = false; 
    public bool isSitDown = false; 

    [Header("音效")]
    [SerializeField] AudioData moveAudioData;
    
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private Vector2 lastMoveDir = Vector2.down; // 預設角色初始面向

    void Update() 
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // 🔹 儲存最後一次有效的移動方向
        if (movement.sqrMagnitude > 0.01f)
        {
            lastMoveDir = movement;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // 🔹 使用 lastMoveDir 設定動畫方向
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFinding", isFinding);
        animator.SetBool("isSitDown", isSitDown);
    }


    void FixedUpdate() 
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier * moveSpeed);
    }
}
