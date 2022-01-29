using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage = 10f;
    public float speed = 0.5f;
    public float lifespan = 3f;
    public bool heatSeeking = false;
    GameObject target = null;
    Vector2 originalTargetPos = Vector2.zero;
    float curLifetime = 0f;

    private void Update() {
        if (curLifetime > lifespan) {
            Destroy(gameObject);
            return;
        }

        if (target != null) {
            //NPCManager npc = target.GetComponent<NPCManager>();
            if (heatSeeking)
                transform.position = Vector2.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
            else
                transform.position = Vector2.Lerp(transform.position, originalTargetPos, Time.deltaTime * speed);

        } else {
            Destroy(gameObject);
        }

        curLifetime += Time.deltaTime;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
        originalTargetPos = target.transform.position;
    }
}
