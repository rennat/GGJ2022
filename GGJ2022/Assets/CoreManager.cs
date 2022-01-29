using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreManager : MonoBehaviour
{
    public float totalHealth = 500f;
    public TMP_Text healthText;

    float curHealth;
    bool dead = false;

    private void Start() {
        curHealth = totalHealth;
        healthText.text = curHealth.ToString();
    }

    public void TakeDamage(float damage) {
        if (dead)
            return;

        curHealth = Mathf.Max(curHealth - damage, 0f);
        healthText.text = curHealth.ToString();

        if (curHealth <= 0f) {
            Die();
        }
    }

    void Die() {
        dead = true;
        Debug.LogError("Game Over");
        GameManager.EndGame();
    }
}
