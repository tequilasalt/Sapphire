using UnityEngine;
using UnityEngine.UI;

public class UIFactory {

    public static Image GetImage(Transform parent, bool native = true, string sprite = null) {

        RectTransform transform = AssetCache.GetAsset<RectTransform>("UIElements/UICanvas");

        Image image = transform.gameObject.AddComponent<Image>();

        image.rectTransform.SetParent(parent);
        image.rectTransform.localPosition = new Vector3(0, 0);
        image.rectTransform.localScale = Vector3.one;
        
        if (sprite != null) {

            image.sprite = AssetCache.GetAsset<Sprite>("Sprites/" + sprite);
            if (native) {
                image.SetNativeSize();
            }
            
        }
        return image;

    }
    
    public static Image GetImage(Transform parent, Vector2 size, string sprite = null) {
        Image image = GetImage(parent, true, sprite);

        image.rectTransform.sizeDelta = size;

        return image;
    }

    public static Image GetImage(Transform parent, Vector2 size, Vector2 pivot, string sprite = null) {
        Image image = GetImage(parent, true, sprite);

        image.rectTransform.sizeDelta = size;

        PivotUtil.SetPivot(image.rectTransform, pivot.x, pivot.y);

        return image;
    }

    public static Text GetText(Transform parent, Vector2 size, int fontSize, Color color, TextAnchor anchor = TextAnchor.MiddleCenter, bool interactable = false, string asset = "UIText") {
        
        Text text = AssetCache.GetAsset<Text>("UIElements/"+ asset);

        text.fontSize = fontSize;
        text.color = color;
        text.rectTransform.SetParent(parent);
        text.rectTransform.sizeDelta = size;

        text.rectTransform.localPosition = new Vector3(0, 0);
        text.rectTransform.localScale = Vector3.one;

        text.alignment = anchor;

        if (!interactable) {
            text.gameObject.AddComponent<IgnoreRaycast>();
        }

        return text;

    }

    public static Text GetText(Transform parent, Vector2 size, int fontSize, Color color, Vector2 pivot, TextAnchor anchor = TextAnchor.MiddleCenter, bool interactable = false, string asset = "UIText") {

        Text text = GetText(parent, size, fontSize, color, anchor, interactable, asset);

        PivotUtil.SetPivot(text.rectTransform, pivot.x, pivot.y);

        return text;

    }
}
