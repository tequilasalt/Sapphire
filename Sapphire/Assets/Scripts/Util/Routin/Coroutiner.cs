using UnityEngine;
using System.Collections;

public class Coroutiner{

    public static Coroutine StartCoroutine(IEnumerator iterationResult) {

        GameObject routeneHandlerGo = new GameObject("Coroutiner");
        CoroutinerInstance routeneHandler = routeneHandlerGo.AddComponent<CoroutinerInstance>();
        
        return routeneHandler.ProcessWork(iterationResult);

    }

}

public class CoroutinerInstance : MonoBehaviour {

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public Coroutine ProcessWork(IEnumerator iterationResult) {
        return StartCoroutine(DestroyWhenComplete(iterationResult));
    }

    public IEnumerator DestroyWhenComplete(IEnumerator iterationResult) {
        yield return StartCoroutine(iterationResult);
        Destroy(gameObject);
    }

}
