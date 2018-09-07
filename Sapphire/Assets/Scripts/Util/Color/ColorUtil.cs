using System.Collections.Generic;
using UnityEngine;

public class ColorUtil {

    private static Dictionary<string, Color> _colorCache;

    public static Color HexToColor(string hex) {

        if (_colorCache == null) {
            _colorCache = new Dictionary<string, Color>();
        }

        if (_colorCache.ContainsKey(hex)) {
            return _colorCache[hex];
        }

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        Color c = new Color32(r, g, b, 255);

        _colorCache.Add(hex, c);

        return c;

    }

    public static Color HexToColor(string hex, float alpha) {
        var c = HexToColor(hex);
        c.a = alpha;

        return c;
    }

    public static Color RgbToColor(int r, int g, int b, int a = 255) {
        return new Color(r/255f, g/255f, b/255f, a/255f);
    }

}
