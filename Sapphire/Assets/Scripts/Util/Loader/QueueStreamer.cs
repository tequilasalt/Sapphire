using System;

public class QueueStreamer {

    private static QueueStreamer _instance;

    private ListQueue<StreamRequest> _queue;
    private StreamRequest _current;
    private bool _busy;

    public QueueStreamer() {
        _queue = new ListQueue<StreamRequest>();
    }

    public bool Busy {
        get { return _busy; }
    }

    public StreamRequest Current {
        get { return _current; }
    }

    public StreamRequest CreateRequest(string url, string destination, StreamRequest.OnLoadCallback callback = null, int buffer = 256 ) {
        
        StreamRequest request = new StreamRequest(url, destination, callback, buffer);
        
        _queue.Enqueue(request);

        Next();

        return request;
    }

    public static QueueStreamer Instance {
        get {
            if (_instance == null) {
                _instance = new QueueStreamer();
            }
            return _instance;
        }
    }

    private void LoadingListener(StreamRequest request, string error) {
        _busy = false;
        Next();
    }

    private void Next() {

        if (_busy) {
            return;
        }

        if (_queue.IsEmpty) {
            _current = null;
            return;
        }

        _current = _queue.Dequeue();

        _current.SetPrivateCallback(LoadingListener);

        Coroutiner.StartCoroutine(_current.Load());

        _busy = true;

    }
}
