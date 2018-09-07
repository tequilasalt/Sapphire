using UnityEngine;

public class PivotUtil {

    public static void SetPivot(RectTransform transform, float pivotMin, float pivotMax) {
        
        transform.pivot = new Vector2(pivotMin, pivotMax);
        transform.anchorMin = new Vector2(pivotMin, pivotMax);
        transform.anchorMax = new Vector2(pivotMin, pivotMax);

    }

}
