using System.Collections.Generic;
using UnityEngine;

public class AssetCache:MonoBehaviour{

    private static Dictionary<string, Object> _assetCache;

    public static T GetAsset<T>(string url) where T : Object {

        if (_assetCache == null) {
            _assetCache = new Dictionary<string, Object>();
        }

        if (_assetCache.ContainsKey(url)) {
            return Instantiate(_assetCache[url] as T);
        }

        _assetCache.Add(url, Resources.Load<T>(url));

        T obj = GetAsset<T>(url);

        return obj;

    }

}
