using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCManager : MonoBehaviour, IPointerClickHandler
{
    public float totalHealth = 100f;
    public float speed = 0.1f;
    public float coreDamage = 1f;
    public float coreDamageTick = 1f;
    public string mode = "wander";
    public float wanderRadius = 0.5f;
    float curHealth;
    float curCoreDamage;

    bool damagingCore = false;

    // Start is called before the first frame update
    void Start() {
        curHealth = totalHealth;
        if (mode == "wander")
            StartCoroutine(WanderCoroutine()); 
        else if (mode == "rush")
            StartCoroutine(MoveCoroutine()); 
    }

    IEnumerator MoveCoroutine() {
        curCoreDamage = coreDamage;

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
            curCoreDamage = 0f;

            Vector2 newTarget = transform.position + new Vector3(Random.Range(-wanderRadius, wanderRadius), Random.Range(-wanderRadius, wanderRadius));

            while (Vector2.Distance(transform.position, newTarget) > 0.1f) {
                transform.position = Vector2.Lerp(transform.position, newTarget, Time.deltaTime * speed);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(0f, 2f));
        }
    }

    IEnumerator FleeCoroutine() {
        curCoreDamage = 0f;

        // Pick a target point
        Vector2 target = UnityEngine.Random.insideUnitCircle.normalized * 20f;
        
        while (Vector2.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * 1f);
            yield return null;
        }
        Die();
        yield return null;
    }

    public void Convert() {
        if (mode == "wander") {
            StopAllCoroutines();
            mode = "rush";
            StartCoroutine(MoveCoroutine());
        }
    }

    public void Disarm() {
        StopAllCoroutines();
        StartCoroutine(FleeCoroutine());
    }

    public void TakeDamage(float damage) {
        curHealth -= damage;
        if (curHealth <= 0)
            Die();

    }

    public void Die() {
        damagingCore = false;
        StopAllCoroutines();
        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        handleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        handleCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        handleCollisionExit(collision.collider);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        handleCollisionExit(collision);
    }

    void handleCollision(Collider2D col) {
        if (col.tag == "Core") {
            if (!damagingCore)
                StartCoroutine(DoCoreDamageCoroutine(col.GetComponent<CoreManager>()));
        }
    }

    void handleCollisionExit(Collider2D col) {
        if (col.tag == "Core") {
            damagingCore = false;
        }
    }

    IEnumerator DoCoreDamageCoroutine(CoreManager core) {
        if (damagingCore || core == null)
            yield break;

        damagingCore = true;
        while (damagingCore) {
            core.TakeDamage(curCoreDamage);
            yield return new WaitForSeconds(coreDamageTick);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Disarm();
    }
}
