using System.Collections;
using System.Collections.Generic;

public class QueueLoader{

    private static QueueLoader _instance;

    private Dictionary<string, LoadRequest> _requestCache;
    private ListQueue<LoadRequest> _queue;

    private bool _busy;
    
    public QueueLoader(){
        _requestCache = new Dictionary<string, LoadRequest>();
        _queue = new ListQueue<LoadRequest>();
        
    }

    public bool Busy{
        get { return _busy; }
    }

    public LoadRequest CreateRequest(string url, FileType type, bool cache = false, LoadRequest.OnLoadCallback callback = null, ArrayList postData = null){

        LoadRequest request;
        
        if(_requestCache.ContainsKey(url)){

            return _requestCache[url];
        }

        request = new LoadRequest(url, type, callback, postData);
                
        if (cache){
            _requestCache.Add(url, request);
        }

        _queue.Enqueue(request);

        Next();
            
        return request;
    }

    public bool ContainsRequest(string url){
        return _requestCache.ContainsKey(url);
    }

    public static QueueLoader Instance{
        get{
            if(_instance == null){
                _instance = new QueueLoader();
            }
            return _instance;
        }
    }

    private void LoadingListener(LoadRequest request, string error){
        _busy = false;
        Next();
    }

    private void Next(){

        if(_busy){
            return;
        }

        if(_queue.IsEmpty){
            return;
        }

        LoadRequest request = _queue.Dequeue();
        
        request.SetPrivateCallback(LoadingListener);

        Coroutiner.StartCoroutine(request.Load());

        _busy = true;

    }
    
}
