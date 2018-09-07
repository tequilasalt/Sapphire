using UnityEngine;
using UnityEngine.UI;

public class Header:AbstractViewElement {

    private Text _header;

    public Header(RectTransform parent) : base(parent) {

        RectTransform.localPosition = Vector3.zero;

        _header = UIFactory.GetText(RectTransform, new Vector2(480, 140), 30, Color.gray, TextAnchor.MiddleCenter, false, "ExoBold");
        _header.supportRichText = true;
        _header.text = "Connection Status: <color=#FF7373>NONE</color>";

    }

    public void SetConnected() {
        _header.text = "Connection Status: <color=#2DB200>ON-LINE</color>";
    }
}

