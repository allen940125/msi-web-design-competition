using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Update = UnityEngine.PlayerLoop.Update;

public class UIFontManager : MonoBehaviour
{
    public static UIFontManager Instance;

    public TMP_FontAsset currentFont;

    private void Awake()
    {
        Instance = this;
        Addressables.LoadAssetAsync<TMP_FontAsset>("Font/GenYoGothic-B SDF").Completed += handle =>
        {
            currentFont = handle.Result;
            currentFont.material = currentFont.material;
        };

    }
    
    public void ApplyFont(TMP_Text text)
    {
        if (currentFont != null)
        {
            text.font = null;
            text.font = currentFont;
            text.ForceMeshUpdate();

            Debug.Log($"[UIFontManager] 成功套用字體: {currentFont.name} 到 {text.name}");
        }
        else
        {
            Debug.LogWarning("[UIFontManager] 字體尚未載入！");
        }
    }

}