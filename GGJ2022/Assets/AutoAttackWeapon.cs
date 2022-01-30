using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackWeapon : MonoBehaviour {
    public WeaponStats weaponStats;
    public float autoTick => 1/weaponStats.RateOfFire;
    public float detectRadius => weaponStats.Range;
    public Projectile projectile;
    public Transform rangePreview;

    private AudioSource audioSource;

    bool attacking = false;

    private void Start() {
        AttackMode();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        rangePreview.localScale = Vector3.one * weaponStats.Range * 2;
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
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip, audioSource.volume);
                    }
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
