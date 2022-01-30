using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public WeaponStats weaponStats;
    public float damage => weaponStats.ProjectileDamage;
    public float speed => weaponStats.ProjectileSpeed;
    public float lifespan => weaponStats.ProjectileLifespan;
    public float aoeRadius => weaponStats.AoeRadius;
    public int aoeLimit => (int)weaponStats.AoeLimit;
    public bool heatSeeking => weaponStats.ProjectileHoming >= 1;
    GameObject target = null;
    Vector3 originalTargetPos = Vector3.zero;
    Vector3 originPos = Vector3.zero;
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
            else {
                //transform.position = Vector2.Lerp(transform.position, originalTargetPos, Time.deltaTime * speed);
                transform.Translate((originalTargetPos - originPos).normalized * Time.deltaTime * speed);
            }

        } else {
            Destroy(gameObject);
        }

        curLifetime += Time.deltaTime;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
        originPos = transform.position;
        originalTargetPos = target.transform.position;
    }

    public void Collided(NPCManager npc) {
        if (aoeRadius > 0f && aoeLimit > 0) {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(npc.transform.position, aoeRadius);

            int aoeReach = 0;
            foreach (Collider2D col in enemiesInRange) {
                if (col.tag == "Enemy") {
                    NPCManager closeNPC = col.GetComponent<NPCManager>();
                    if (closeNPC.mode == "flee" || closeNPC == null)
                        continue;

                    if (closeNPC.openUmbrella != null)
                        closeNPC.openUmbrella.SetActive(true);
                    closeNPC.Disarm();
                    aoeReach += 1;
                    if (aoeReach >= aoeLimit)
                        break;
                }
            }
        }
        Destroy(gameObject);
    }
}
