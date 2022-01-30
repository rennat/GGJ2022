using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : MonoBehaviour
{
    public GameObject aura;
    public float rotateSpeed = 2f;
    public float radius = 1f;
    public float cooldown = 1f;
    bool attacking = true;

    private void Start() {
        StartCoroutine(attackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
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
