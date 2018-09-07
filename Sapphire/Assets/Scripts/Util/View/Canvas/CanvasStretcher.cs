using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(RectTransform))]
public class CanvasStretcher:MonoBehaviour {

    public bool Continious;

    public int ReferenceWidth;
    public int ReferenceHeight;

    private float _distance;
    private RectTransform _rect;

    void Start() {
        _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        _rect = gameObject.GetComponent<RectTransform>();

        Strech();
    }

    void Update() {

        if (Continious) {
            Strech();
        }

    }

    public void Strech() {

        var frustumHeight = 2.0f * _distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * Camera.main.aspect;

        _rect.localScale = new Vector3(frustumWidth / ReferenceWidth, frustumHeight / ReferenceHeight, frustumWidth / ReferenceWidth);

    }
}
