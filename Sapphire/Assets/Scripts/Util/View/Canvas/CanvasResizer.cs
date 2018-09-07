using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResizer : MonoBehaviour {

    private static CanvasResizer _instance;

    public bool Continious;

    public int ReferenceWidth;
    public int ReferenceHeight;

    private float _distance;
    private float _ratio;

    private RectTransform _rect;

    private Image _leftBlocker;
    private Image _rightBlocker;

    void Awake() {
        _instance = this;
    }

	public void Init (bool continious) {

	    Continious = continious;

	    _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
	    _rect = gameObject.GetComponent<RectTransform>();

        Resize();
	}

    public Vector2 Size {
        get {
             return new Vector2(1080, _rect.sizeDelta.y);
        }
    }

    public void Resize() {

        _ratio = (float)Screen.width / (float)Screen.height;

        if (_ratio > 0.5625f) {

            _rect.sizeDelta = new Vector2(ReferenceHeight*_ratio, ReferenceHeight);

            float frustumHeight = 2.0f * _distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;

            _rect.localScale = new Vector3(frustumWidth / (ReferenceHeight*_ratio), frustumHeight / ReferenceHeight, frustumHeight / ReferenceHeight);

        }
        else {
            _rect.sizeDelta = new Vector2(ReferenceWidth, ReferenceWidth/_ratio);

            float frustumHeight = 2.0f * _distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;

            _rect.localScale = new Vector3(frustumWidth / ReferenceWidth, frustumHeight / (ReferenceWidth/_ratio), frustumWidth / ReferenceWidth);
        }

    }

    void Update () {
	
        if (Continious) {
            Resize();
        }

    }

    public static CanvasResizer Instance {
        get { return _instance; }
    }
}
