using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreManager : MonoBehaviour
{
    public float totalHealth = 500f;
    public TMP_Text healthText;

    float curHealth;

    private void Start() {
        curHealth = totalHealth;
        healthText.text = curHealth.ToString();
    }

    public void TakeDamage(float damage) {
        curHealth -= damage;
        healthText.text = curHealth.ToString();

        if (curHealth <= 0f) {
            Die();
        }
    }

    void Die() {
        Debug.LogError("Game Over");
    }
}
