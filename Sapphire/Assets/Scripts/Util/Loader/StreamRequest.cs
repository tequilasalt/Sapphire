using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class StreamRequest {

    private string _url;
    private string _destination;

    private bool _isDone;

    private OnLoadCallback _publicCallback;
    private OnLoadCallback _privateCallback;

    private UnityWebRequest _webRequest;
    private int _buffer;

    public delegate void OnLoadCallback(StreamRequest request, string error);

    public StreamRequest(string url, string destination, OnLoadCallback publicCallback, int buffer = 256) {

        _buffer = buffer;
        _url = url;
        _destination = destination;
        _publicCallback = publicCallback;

    }

    public string Url {
        get { return _url; }
    }

    public void SetPrivateCallback(OnLoadCallback callback) {
        _privateCallback = callback;
    }

    public IEnumerator Load() {
        
        _webRequest = new UnityWebRequest(_url);

        _webRequest.downloadHandler = new ToFileDownloadHandler(new byte[_buffer*1024], _destination);
        _webRequest.Send();

        while (!_webRequest.isDone) {
            yield return null;
        }

        if (string.IsNullOrEmpty(_webRequest.error)) {
             Done();   
        } else {
            _publicCallback(null, _webRequest.error);
            _privateCallback(this, _webRequest.error);
        }

    }

    public float Progress {
        get { return ((ToFileDownloadHandler) _webRequest.downloadHandler).Progress; }
    }

    public bool IsDone {
        get { return _isDone; }
    }

    public void Stop() {
        _privateCallback(this, null);

        ((ToFileDownloadHandler)_webRequest.downloadHandler).Cancel();
        ((ToFileDownloadHandler)_webRequest.downloadHandler).Dispose();
       
    }

    private void Done() {
        _isDone = true;

        _privateCallback(this, null);
        _publicCallback(this, null);

    }

    class ToFileDownloadHandler : DownloadHandlerScript {

        private int _expected = -1;
        private int _received = 0;
        private string _filepath;
        private FileStream _fileStream;
        private bool _canceled = false;

        public ToFileDownloadHandler(byte[] buffer, string filepath): base(buffer) {
            _filepath = filepath;
            _fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
        }

        protected override byte[] GetData() { return null; }

        protected override bool ReceiveData(byte[] data, int dataLength) {

            if (data == null || data.Length < 1) {
                return false;
            }
            
            _received += dataLength;

            if (!_canceled) {
                _fileStream.Write(data, 0, dataLength);
            }

            //Debug.Log("progress ->" + GetProgress()+" :"+dataLength);
            
            return true;
        }

        protected override float GetProgress() {
            if (_expected < 0) return 0;
            return (float)_received / _expected;
        }
        
        public float Progress {
            get {
                return GetProgress();
            }
        }

        protected override void CompleteContent() {
            _fileStream.Close();
        }

        protected override void ReceiveContentLength(int contentLength) {
            _expected = contentLength;
        }

        public void Cancel() {
            
            _canceled = true;
            _fileStream.Close();
            
        }

    }
}
