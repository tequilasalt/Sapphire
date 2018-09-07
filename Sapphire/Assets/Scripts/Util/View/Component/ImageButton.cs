using UnityEngine;
using UnityEngine.UI;

public class ImageButton:AbstractClickable {

    protected Image _buttonImage;
    protected Image _expand;

    private int _expandSize;
    
    public ImageButton(string sprite, int expand = 0) {
        Init(sprite, expand);
    }

    public ImageButton(RectTransform parent, string sprite, int expand = 0) : base(parent) {
        Init(sprite, expand);
    }

    protected virtual void Init(string sprite, int expand) {

        _expandSize = expand;
        _buttonImage = RectTransform.gameObject.AddComponent<Image>();

        ChangeSprite(sprite);
        
    }

    public Color Color {
        get { return _buttonImage.color; }
        set { _buttonImage.color = value; }
    }

    public void ChangeSprite(string sprite) {

        if (!string.IsNullOrEmpty(sprite)) {

            _buttonImage.sprite = AssetCache.GetAsset<Sprite>("Sprites/" + sprite);

            _buttonImage.SetNativeSize();

            _expand = UIFactory.GetImage(RectTransform, _buttonImage.rectTransform.sizeDelta + Vector2.one * _expandSize);
            _expand.rectTransform.localPosition = Vector3.zero;
            _expand.color = new Color(1, 1, 1, 0.0001f);
        }
    }

    public override Vector2 Size {
        get { return RectTransform.sizeDelta; }
        set { RectTransform.sizeDelta = value; }
    }

}
