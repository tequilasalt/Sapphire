using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractViewElement {

    private RectTransform _rectTransform;
    private bool _visible = true;

    protected AbstractViewElement() {

        _rectTransform = AssetCache.GetAsset<RectTransform>("UIElements/UICanvas");
        _rectTransform.gameObject.name = GetType().FullName;

    }

    protected AbstractViewElement(RectTransform parent) {

        _rectTransform = AssetCache.GetAsset<RectTransform>("UIElements/UICanvas");
        _rectTransform.gameObject.name = GetType().FullName;

        _rectTransform.SetParent(parent);

    }

    public virtual bool Visible {

        get { return _visible; }
        set {

            _visible = value;

            MaskableGraphic[] images = _rectTransform.GetComponentsInChildren<MaskableGraphic>(true);

            foreach (MaskableGraphic image in images) {
                image.enabled = _visible;
            }

        }
    }

    public virtual Vector2 Size {
        get { return _rectTransform.sizeDelta; }
        set { _rectTransform.sizeDelta = value; }
    }

    public RectTransform RectTransform {
        get { return _rectTransform; }
    }

}

