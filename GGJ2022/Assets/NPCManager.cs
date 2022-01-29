using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public float speed = 0.1f;
    public float coreDamage = 1f;
    public string mode = "wander";
    public float wanderRadius = 0.5f;

    // Start is called before the first frame update
    void Start() {
        if (mode == "wander")
            StartCoroutine(WanderCoroutine()); 
        else if (mode == "rush")
            StartCoroutine(MoveCoroutine()); 
    }

    IEnumerator MoveCoroutine() {
        // Pick a target point
        Vector2 target = Vector2.zero;
        
        while (Vector2.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
        yield return null;
    }

    IEnumerator WanderCoroutine() {
        while (mode == "wander") {
            Vector2 newTarget = transform.position + new Vector3(Random.Range(-wanderRadius, wanderRadius), Random.Range(-wanderRadius, wanderRadius));

            while (Vector2.Distance(transform.position, newTarget) > 0.1f) {
                transform.position = Vector2.Lerp(transform.position, newTarget, Time.deltaTime * speed);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(0f, 2f));
        }
    }

    public void convert() {
        if (mode == "wander") {
            StopAllCoroutines();
            mode = "rush";
            StartCoroutine(MoveCoroutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        handleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        handleCollision(collision);
    }

    void handleCollision(Collider2D col) {
        if (col.tag == "Core") {
            col.GetComponent<CoreManager>().TakeDamage(coreDamage);
        }
    }
}
