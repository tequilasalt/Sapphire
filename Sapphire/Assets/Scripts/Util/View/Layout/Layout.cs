using UnityEngine;
using System.Collections;

public class Layout : AbstractLayout{

    public Layout() {
    }

    protected override IEnumerator Opening(bool instant = false, bool reverse = false) {
        OnOpen();
        yield break;
    }

    protected override IEnumerator Closing(bool instant = false, bool reverse = false) {
        OnClose();
        yield break;
    }

    public override void OnResize() {
    }

}
