using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler{

    private AbstractClickable _clickable;
    private bool _enter;
    
    public void Init(AbstractClickable master) {
        _clickable = master;
    }

    public void OnPointerDown(PointerEventData eventData) {
        _clickable.OnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData) {
        _clickable.OnPointerUp();
    }

    public void OnPointerClick(PointerEventData eventData) {
        _clickable.OnPointerClick();
    }

}