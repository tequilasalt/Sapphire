using UnityEngine;
using System.Collections;

public class FixedUpdater : MonoBehaviour {

    private IUpdateProxy _proxy;

    public void Init(IUpdateProxy proxy) {
        _proxy = proxy;
    }

    void FixedUpdate() {
        if (_proxy != null) {
            _proxy.Update();
        }
    }

}
