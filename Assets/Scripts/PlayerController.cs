using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        // 取得水平與垂直輸入
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize(); // 防止斜向移動變快

        // 如果有移動，就根據移動方向更新旋轉（假設預設朝向右邊）
        if (movement != Vector2.zero) {
            // 計算角度，將弧度轉換成角度（以Z軸為旋轉軸）
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void FixedUpdate() 
    {
        Debug.Log(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier);
        rb.linearVelocity = movement * (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMovementMultiplier * moveSpeed);
    }
}