using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackWeapon : MonoBehaviour {
    public WeaponStats weaponStats;
    public float autoTick => weaponStats.RateOfFire;
    public float detectRadius => weaponStats.Range;
    public Projectile projectile;

    bool attacking = false;

    private void Start() {
        AttackMode();
    }

    public void AttackMode() {
        if (attacking)
            return;

        attacking = true;
        StartCoroutine(autoAttack());
    }

    IEnumerator autoAttack() {
        while (attacking) {
            for (int i = 0; i < weaponStats.ProjectileCount; i++)
            {
                // Find closest enemy
                GameObject closestEnemy = findClosestEnemy();

                // Attack
                if (closestEnemy != null)
                {
                    GameObject go = Instantiate(projectile.gameObject);
                    go.transform.position = transform.position;
                    go.GetComponent<Projectile>().SetTarget(closestEnemy);
                }

                // Wait
                yield return new WaitForSeconds(autoTick);
            }
        }
    }

    GameObject findClosestEnemy() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        float closestDist = Mathf.Infinity;
        GameObject closestGo = null;
        foreach (Collider2D col in enemiesInRange) {
            if (col.tag == "Enemy") {
                NPCManager npc = col.GetComponent<NPCManager>();
                if (npc.mode == "flee" || npc == null)
                    continue;

                float dist = Vector2.Distance(transform.position, col.transform.position);
                if (dist < closestDist) {
                    closestDist = dist;
                    closestGo = col.gameObject;
                }
            }
        }

        return closestGo;
    }
}
