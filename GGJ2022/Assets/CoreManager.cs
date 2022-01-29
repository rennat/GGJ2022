using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public float totalHealth = 500f;

    float curHealth;

    private void Start() {
        curHealth = totalHealth;
    }

    public void TakeDamage(float damage) {
        curHealth -= damage;
        Debug.LogError(curHealth);

        if (curHealth <= 0f) {
            Die();
        }
    }

    void Die() {
        Debug.LogError("Game Over");
    }
}
