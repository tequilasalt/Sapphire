using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockerLayout : Layout {

    public BlockerLayout() {
    }

    public bool Blocking {
        get { return _maskImage.raycastTarget; }
        set { _maskImage.raycastTarget = value; }
    }
}
