public class LayoutLayer : AbstractViewElement {

    private string _layerType;
    private AbstractLayout _current;
    private bool _switching;

    public LayoutLayer(string layer) {
        _layerType = layer;
        RectTransform.gameObject.name = _layerType;
    }

    public string LayerType {
        get { return _layerType; }
    }

    public AbstractLayout Current {
        get { return _current; }
        set { _current = value; }
    }

    public T GetCurrent<T>() where T : AbstractLayout {
        return _current as T;
    }
    
    public bool Switching {
        get { return _switching; }
        set { _switching = value; }
    }

}