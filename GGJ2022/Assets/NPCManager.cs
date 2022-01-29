using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(MoveCoroutine()); 
    }

    IEnumerator MoveCoroutine() {
        // Pick a target point
        Vector3 target = Vector3.zero;
        
        while (Vector2.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
        yield return null;
    }
}
