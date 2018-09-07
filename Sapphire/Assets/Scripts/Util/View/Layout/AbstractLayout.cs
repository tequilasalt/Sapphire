using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractLayout:AbstractViewElement {

    private VoidCallback _openCallback;
    private VoidCallback _closeCallback;

    protected RectTransform _container;
    protected Image _maskImage;
    protected Mask _mask;

    protected object[] _layoutData;

    protected AbstractLayout() {

        _container = AssetCache.GetAsset<RectTransform>("UIElements/UICanvas");
        _container.transform.SetParent(RectTransform);
        _container.transform.localPosition = Vector3.zero;
        _container.transform.localScale = Vector3.one;
        _container.name = "Container";

        _maskImage = _container.gameObject.AddComponent<Image>();

        _mask = _container.gameObject.AddComponent<Mask>();
        _mask.showMaskGraphic = false;

        _container.sizeDelta = CanvasResizer.Instance.Size; //new Vector2(1080, 1920);

    }

    public virtual void Init(params object[] layoutData) {
        _layoutData = layoutData;
    }
    
    public virtual void Open(bool instant = false, bool reverse = false, VoidCallback openCallback = null) {
        _openCallback = openCallback;
        Coroutiner.StartCoroutine(Opening(instant, reverse));
    }

    public virtual void Close(bool instant = false, bool reverse = false, VoidCallback closeCallback = null) {
        _closeCallback = closeCallback;
        Coroutiner.StartCoroutine(Closing(instant, reverse));
    }

    protected abstract IEnumerator Opening(bool instant = false, bool reverse = false);

    protected abstract IEnumerator Closing(bool instant = false, bool reverse = false);

    protected virtual void OnOpen() {

        if (_openCallback == null) {
            return;
        }

        _openCallback();
        _openCallback = null;
    }

    protected virtual void OnClose() {

        if (_closeCallback == null) {
            return;
        }

        _closeCallback();
        _closeCallback = null;
    }

    public abstract void OnResize();
    
}
