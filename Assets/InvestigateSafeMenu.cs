using UnityEngine;

public class InvestigateSafeMenu : MonoBehaviour
{
    void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            // 排除滑鼠按鍵 (Mouse0 ~ Mouse6)
            if (key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6)
                continue;

            if (Input.GetKeyDown(key))
            {
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
