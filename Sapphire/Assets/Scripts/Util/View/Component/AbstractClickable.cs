using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractClickable:AbstractViewElement {

    public delegate void OnInteraction();
    public delegate void OnParameterInteraction(object parameter);

    private class StoredCallback {

        public object Parameter;
        public OnInteraction Callback;
        public OnParameterInteraction ParameterCallback;

        public StoredCallback(object parameter, OnInteraction callback = null, OnParameterInteraction parameterCallback = null) {

            Parameter = parameter;
            Callback = callback;
            ParameterCallback = parameterCallback;
        }
    }

    protected bool _enabled = true;

    private object _downParameter;
    private object _upParameter;
    private object _clickParameter;

    private PointerEventHandler _handler;

    private Dictionary<InteractionType, StoredCallback> _callbacks;
    
    public enum InteractionType {
        CLICK,
        DOWN,
        UP
    }

    protected AbstractClickable() {
    }

    protected AbstractClickable(RectTransform parent) : base(parent) {
    }

    public virtual bool Enabled {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public void RegisterCallback(InteractionType type, OnInteraction callback) {

        if (_callbacks == null) {
            _callbacks = new Dictionary<InteractionType, StoredCallback>();
        }

        if (_handler == null) {
            _handler = RectTransform.gameObject.AddComponent<PointerEventHandler>();
            _handler.Init(this);
        }

        if (_callbacks.ContainsKey(type)) {
            _callbacks[type].Callback = callback;
        }
        else {
            _callbacks.Add(type, new StoredCallback(null, callback));
        }
        
    }

    public void RegisterCallback(InteractionType type, OnParameterInteraction callback, object parameter) {

        if (_callbacks == null) {
            _callbacks = new Dictionary<InteractionType, StoredCallback>();
        }

        if (_handler == null) {
            _handler = RectTransform.gameObject.AddComponent<PointerEventHandler>();
            _handler.Init(this);
        }

        _callbacks.Add(type, new StoredCallback(parameter, null, callback));
    }

    public void UnregisterCallback(InteractionType type) {
        if (_callbacks.ContainsKey(type)) {
            _callbacks.Remove(type);
        }
    }

    public void OnPointerDown() {
        HandlePointerEvent(InteractionType.DOWN);
    }

    public void OnPointerUp() {
        HandlePointerEvent(InteractionType.UP);
    }

    public void OnPointerClick() {
        HandlePointerEvent(InteractionType.CLICK);
    }

    protected virtual void HandlePointerEvent(InteractionType type) {

        if (_handler == null) {
            return;
        }

        if (!_enabled) {
            return;
        }

        if (!_callbacks.ContainsKey(type)) {
            return;
        }

        if (_callbacks[type].Callback == null) {
            _callbacks[type].ParameterCallback(_callbacks[type].Parameter);
        }
        else {
            _callbacks[type].Callback();
        }
        
    }
    
}
