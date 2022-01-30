using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : MonoBehaviour
{
    public WeaponStats weaponStats;
    public float rotateSpeed => weaponStats.WeaponSpeed;
    public float radius => weaponStats.WeaponSize;
    public float cooldown => weaponStats.Cooldown;
    bool attacking = true;

    public Transform rangePreview;

    private void Start() {
        StartCoroutine(attackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        rangePreview.localScale = Vector3.one * radius * 2f;
    }

    IEnumerator attackCoroutine() {
        while (attacking) {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in enemiesInRange) {
                if (col.tag == "Enemy") {
                    NPCManager closeNPC = col.GetComponent<NPCManager>();
                    if (closeNPC.mode == "flee" || closeNPC == null)
                        continue;

                    if (closeNPC.openUmbrella != null)
                        closeNPC.openUmbrella.SetActive(true);
                    closeNPC.Disarm();
                }
            }

            yield return new WaitForSeconds(cooldown);
        }
    }
}
