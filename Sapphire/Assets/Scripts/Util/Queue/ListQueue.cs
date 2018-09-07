using System.Collections.Generic;
using System;

public class ListQueue<T> {

    protected List<T> _list;

    public ListQueue() {
    }

    public List<T> List {
        get { return _list; }
    }

    public void AddToTop(T n){
        if (_list == null) {
            _list = new List<T>();
        }

        _list.Insert(0, n);
    }

    public void Enqueue(T n) {

        if (_list == null) {
            _list = new List<T>();
        }

        _list.Add(n);
    }

    public T Dequeue() {

        if (IsEmpty) {
            throw new Exception("Queue is empty");
        }

        T n = _list[0];
        _list.RemoveAt(0);

        return n;
    }

    public T Next() {

        if (IsEmpty) {
            throw new Exception("Queue is empty");
        }
        
        T n = _list[0];

        return n;
    }

    public void Clear() {
        _list = null;
    }

    public int Size {
        get {

            if (_list == null) {
                return 0;
            }
            
            return _list.Count;
        }
    }

    public bool IsEmpty {
        get { return Size == 0; }
    }

}



