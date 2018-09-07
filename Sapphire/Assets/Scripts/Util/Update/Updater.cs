using UnityEngine;

public class Updater:MonoBehaviour {

    private IUpdateProxy _proxy; 

    public void Init(IUpdateProxy proxy) {
        _proxy = proxy;
    }

    void Update() {
        if (_proxy != null) {
            _proxy.Update();
        }
    }
}
