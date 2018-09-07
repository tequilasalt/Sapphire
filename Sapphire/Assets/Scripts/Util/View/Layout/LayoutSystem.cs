using System.Collections.Generic;
using UnityEngine;

public class LayoutSystem : MonoBehaviour {

    public enum SwitchType {
        FIRST_OPEN_NEW,
        FIRST_CLOSE_OLD,
        JUST_CLOSE_OLD,
        JUST_OPEN_NEW,
        SAME_TIME,
        URGENT
    }

    private static LayoutSystem _instance;

    private int _width = 0;
    private int _height = 0;

    private Dictionary<string, LayoutLayer> _layers;

    void Awake() {
        _instance = this;
        gameObject.AddComponent<AssetCache>();
    }
    
	void Update () {

        if (_layers != null) {

            if (Screen.width != _width || Screen.height != _height) {

                _width = Screen.width;
                _height = Screen.height;
                
                foreach (KeyValuePair<string, LayoutLayer> layerPair in _layers) {

                    if (layerPair.Value.Current != null) {
                        layerPair.Value.Current.OnResize();
                    }

                }
            }
        }

    }

    public void Init(params string[] layers) {

        _layers = new Dictionary<string, LayoutLayer>();

        int len = layers.Length;

        for (int i = 0; i < len; i++) {
            CreateLayer(layers[i]);
        }

    }

    public Dictionary<string, LayoutLayer> Layers {
        get { return _layers; }
    }

    public T GetLayout<T>(string layer) where T : AbstractLayout {
        return (T) GetLayout(layer);
    }

    public AbstractLayout GetLayout(string layer) {
        return _layers[layer].GetCurrent<AbstractLayout>();
    }

    public void SwitchLayout<T>(string layer, SwitchType switchType, bool reverse = false, params object[] layoutData) where T : AbstractLayout, new() {

        AbstractLayout currentLayout;
        AbstractLayout temporaryLayout = null;

        LayoutLayer targetLayer = _layers[layer];

        if (targetLayer.Switching) {
            return;
        }

        if (switchType != SwitchType.JUST_CLOSE_OLD) {
            temporaryLayout = new T();
        }

        currentLayout = GetLayout(layer);

        if (temporaryLayout != null) {

            temporaryLayout.RectTransform.SetParent(targetLayer.RectTransform);
            temporaryLayout.RectTransform.localPosition = Vector3.zero;
            temporaryLayout.RectTransform.localScale = Vector3.one;

            temporaryLayout.Init(layoutData);

        }

        switch (switchType) {

            case SwitchType.URGENT:

                if (currentLayout != null) {
                    targetLayer.Current = null;
                    currentLayout.Close(true, false, delegate {
                        DestroyTemporaryLayout(currentLayout);
                    });
                }
                
                if (temporaryLayout != null) {
                    temporaryLayout.Open(true, false, delegate {
                        targetLayer.Current = temporaryLayout;

                        
                    });
                }

                break;

            case SwitchType.JUST_OPEN_NEW:

                if (temporaryLayout == null) {
                    
                    return;
                }

                if (currentLayout != null) {
                    DestroyTemporaryLayout(temporaryLayout);
                    SwitchLayout<T>(layer, SwitchType.FIRST_OPEN_NEW, reverse, layoutData);
                    return;
                }
                targetLayer.Switching = true;
                
                temporaryLayout.Open(false, reverse, delegate {
                    targetLayer.Current = temporaryLayout;
                    targetLayer.Switching = false;
                    
                });

                break;

            case SwitchType.JUST_CLOSE_OLD:

                if (temporaryLayout != null) {
                    DestroyTemporaryLayout(temporaryLayout);
                    SwitchLayout<T>(layer, SwitchType.FIRST_CLOSE_OLD, reverse, layoutData);
                    return;
                }

                if (currentLayout == null) {
                    
                    return;
                }
                targetLayer.Switching = true;
                currentLayout.Close(false, reverse, delegate {
                    targetLayer.Current = null;
                    targetLayer.Switching = false;
                    DestroyTemporaryLayout(currentLayout);
                    
                });

                break;

            case SwitchType.FIRST_CLOSE_OLD:

                if (temporaryLayout == null) {
                    SwitchLayout<T>(layer, SwitchType.JUST_CLOSE_OLD, reverse, layoutData);
                    return;
                }

                if (currentLayout == null) {
                    DestroyTemporaryLayout(temporaryLayout);
                    SwitchLayout<T>(layer, SwitchType.JUST_OPEN_NEW, reverse, layoutData);
                    return;
                }
                targetLayer.Switching = true;
                currentLayout.Close(false, reverse, delegate {
                    targetLayer.Current = null;
                    DestroyTemporaryLayout(currentLayout);
                    
                    temporaryLayout.Open(false, reverse, delegate {
                        targetLayer.Current = temporaryLayout;
                        targetLayer.Switching = false;

                        
                    });
                });

                break;
            case SwitchType.FIRST_OPEN_NEW:

                if (temporaryLayout == null) {
                    SwitchLayout<T>(layer, SwitchType.JUST_CLOSE_OLD, reverse, layoutData);
                    return;
                }

                if (currentLayout == null) {
                    DestroyTemporaryLayout(temporaryLayout);
                    SwitchLayout<T>(layer, SwitchType.JUST_OPEN_NEW, reverse, layoutData);
                    return;
                }
                targetLayer.Switching = true;
                
                temporaryLayout.Open(false, reverse, delegate {
                    targetLayer.Current = null;
                    DestroyTemporaryLayout(currentLayout);

                    currentLayout.Close(false, reverse, delegate {
                        targetLayer.Current = temporaryLayout;
                        targetLayer.Switching = false;

                        
                    });
                });

                break;
            case SwitchType.SAME_TIME:

                targetLayer.Switching = true;

                if (currentLayout != null) {
                    currentLayout.Close(false, reverse, delegate {
                        targetLayer.Current = null;
                        DestroyTemporaryLayout(currentLayout);
                        
                    });
                }
                
                if (temporaryLayout != null) {
                    temporaryLayout.Open(false, reverse, delegate {
                        targetLayer.Current = temporaryLayout;
                        targetLayer.Switching = false;

                    });
                }

                break;
        }

    }

    public void CloseCurrent(string layer, bool reverse = false) {

        AbstractLayout currentLayout = GetLayout(layer);

        if (currentLayout == null) {
            return;
        }

        LayoutLayer targetLayer = _layers[layer];

        targetLayer.Switching = true;

        currentLayout.Close(false, reverse, delegate {

            targetLayer.Current = null;
            targetLayer.Switching = false;
            DestroyTemporaryLayout(currentLayout);

        });
    }

    private void DestroyTemporaryLayout(AbstractLayout layout) {
        if (layout != null) {
            GameObject.Destroy(layout.RectTransform.gameObject);
        }
    }

    private void CreateLayer(string layerType) {

        LayoutLayer layer = new LayoutLayer(layerType);

        _layers.Add(layerType, layer);

        layer.RectTransform.SetParent(transform);
        layer.RectTransform.localPosition = Vector3.zero;
        layer.RectTransform.localScale = Vector3.one;

    }

    public static LayoutSystem Instance {
        get { return _instance; }
    }

}
