using UnityEngine;
using System.Collections;

public class LateUpdater : MonoBehaviour{

    private IUpdateProxy _proxy;

    public void Init(IUpdateProxy proxy){
        _proxy = proxy;
    }

    void LateUpdate(){
        if (_proxy != null){
            _proxy.Update();
        }
    }

}
