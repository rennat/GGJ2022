using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NPCManager : MonoBehaviour, IPointerClickHandler
{
    public float totalHealth = 100f;
    public float speed = 0.1f;
    public float coreDamage = 1f;
    public float coreDamageTick = 1f;
    public float playerDamage = 2f;
    public string mode = "wander";
    public float wanderRadius = 0.5f;
    float curHealth;
    float curCoreDamage;
    float curPlayerDamage;

    bool damagingCore = false;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start() {
        curHealth = totalHealth;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (mode == "wander")
            StartCoroutine(WanderCoroutine()); 
        else if (mode == "rush")
            StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine() {
        curCoreDamage = coreDamage;
        curPlayerDamage = playerDamage;

        // Pick a target point
        Vector2 target = Vector2.zero;
        agent.SetDestination(target);
        
        while (Vector2.Distance(transform.position, target) > 0.1f) {
            //transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
        yield return null;
    }

    IEnumerator WanderCoroutine() {
        while (mode == "wander") {
            curCoreDamage = 0f;
            curPlayerDamage = 0f;

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
        curPlayerDamage = 0f;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<NavMeshAgent>());

        Color baseColor = GetComponentInChildren<SpriteRenderer>().color;
        GetComponentInChildren<SpriteRenderer>().color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.25f);

        // Pick a target point
        Vector2 target = UnityEngine.Random.insideUnitCircle.normalized * 9f;

        float deathTimout = 10f;
        float curFleeTime = 0f;
        while (Vector2.Distance(transform.position, target) > 0.1f && curFleeTime < deathTimout) {
            curFleeTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * 3f);
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
        if (mode == "flee")
            return;
        mode = "flee";

        StopAllCoroutines();
        StartCoroutine(FleeCoroutine());
    }

    public void TakeDamage(float damage) {
        curHealth -= damage;
        if (curHealth <= 0)
            Die();

    }

    public void Die() {
        if (agent != null)
            agent.isStopped = true;
        damagingCore = false;
        StopAllCoroutines();

        Destroy(gameObject);
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
        } else if (col.tag == "Projectile") {
            Projectile projectile = col.GetComponent<Projectile>();
            Disarm();
            Destroy(projectile.gameObject);

            if (PlayerInfo.instance != null)
                PlayerInfo.instance.GainXP();
        } else if (col.tag == "Player") {
            if (PlayerInfo.instance != null)
                PlayerInfo.instance.DamagePlayer(curPlayerDamage);
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
