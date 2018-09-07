using System.Collections;
using UnityEngine;

public class LoadRequest{

    private WWW _www;
    private string _url;
    private FileType _type;

    private string _text;
    private AssetBundle _bundle;
    private Texture2D _texture;
    private AudioClip _audio;
        
    private bool _isDone;

    private ArrayList _postData;
    private byte[] _byte;

    private OnLoadCallback _publicCallback;
    private OnLoadCallback _privateCallback;

    public delegate void OnLoadCallback(LoadRequest request, string error);

    public LoadRequest(string url, FileType type, OnLoadCallback callback, ArrayList postData = null){

        _url = url;
        _publicCallback = callback;
        _type = type;
        _postData = postData;

    }
    
    public FileType Type{
        get { return _type; }
    }

    public string Url{
        get { return _url; }
    }

    public bool IsDone{
        get { return _isDone; }
    }

    public AssetBundle Asset{
        get{
            if(!_isDone){
                return null;
            }
            return _bundle;
        }
    }

    public string Text{
        get{

            if (!_isDone) {
                return null;
            }
                
            return _text;
        }
    }

    public byte[] Bytes {
        get {

            if (!_isDone) {
                return null;
            }
            
            return _byte;
        }
    }

    public Texture2D Texture{
        get{

            if (!_isDone) {
                return null;
            }
                
            return _texture;
        }
    }

    public AudioClip Audio {
        get {

            if (!_isDone) {
                return null;
            }

            return _audio;
        }
    }

    public void SetPrivateCallback(OnLoadCallback callback) {
        _privateCallback = callback;
    }

    public IEnumerator Load(){
        
        if(_postData != null && _type == FileType.TEXT){

            WWWForm form = new WWWForm();
            
            Debug.Log(SimpleJSON.JSON.Serialize(_postData));

            form.AddField("parameters", SimpleJSON.JSON.Serialize(_postData));
           
            _www = new WWW(_url, form);

        }else{
            _www = new WWW(_url);
        }
        
        yield return _www;

        if (_www.error != null) {
            Debug.Log("Load Error: "+_www.error+" url:"+_url);

            _publicCallback(null, _www.error);
            _privateCallback(this, _www.error);

            yield break;
        }

        if (_isDone) {
            yield break;
        }
        
        switch (_type){
            case FileType.TEXT:
                _text = _www.text;
                break;
            case FileType.ASSET_BUNDLE:
                _bundle = _www.assetBundle;
                break;
            case FileType.TEXTURE:
                _texture = _www.texture;
                break;
            case FileType.BYTE:
                _byte = _www.bytes;
                break;
            case FileType.AUDIO:
                _audio = _www.GetAudioClip();
                break;
        }
            
        Done();
    }

    public float Progress {
        get {

            if (_www == null) {
                return 0;
            }

            return _www.progress;
        }
    } 

    public void Dispose(bool assetBundle = true) {

        if (assetBundle) {

            if (_type == FileType.ASSET_BUNDLE) {
                _www.assetBundle.Unload(true);
            }
        }
        
        _www.Dispose();
    }

    public void Stop() {
        _privateCallback(this, null);
        _www.Dispose();
    }

    private void Done(){
        _isDone = true;

        _privateCallback(this, null);
        _publicCallback(this, null);
        
    }

}


