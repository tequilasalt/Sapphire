using System.Collections;
using UnityEngine;

public class Delayed {

    public static void Call(float delay, VoidCallback call) {
        Coroutiner.StartCoroutine(CallDelayed(delay, call));
    }

    private static IEnumerator CallDelayed(float delay, VoidCallback call) {
        yield return new WaitForSeconds(delay);

        call();
    }

}
