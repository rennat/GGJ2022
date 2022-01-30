using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreManager : MonoBehaviour
{
    public float totalHealth = 500f;

    float curHealth;
    bool dead = false;

    private void Start() {
        curHealth = totalHealth;
        UIController.instance.coreHealthSlider.maxValue = totalHealth;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        UIController.instance.coreHealthSlider.value = curHealth;
        UIController.instance.coreHealthText.text = curHealth.ToString() + " / " + totalHealth.ToString();
    }

    public void TakeDamage(float damage) {
        AudioManager.CoreTakesDamage();
        if (dead)
            return;

        curHealth = Mathf.Max(curHealth - damage, 0f);
        UpdateHealthUI();

        if (curHealth <= 0f) {
            Die();
        }
    }

    void Die() {
        dead = true;
        GameManager.EndGame();
    }
}
