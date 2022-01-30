using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

    public float playerMineSpeed = 1f;
    public float currentHealth;
    public float currentXP = 0f;
    public int maxHealth;
    public int wood = 0;
    public float levelBaseXP = 500f;
    public float levelXPMultiplier = 1.2f;

    int curLevel = 1;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Machine singleton is already initialized");
            Destroy(gameObject);
        }
        else if (instance != this)
        {
            instance = this;
        }
    }

    float getNextLevelXP() {
        return Mathf.Floor(levelBaseXP * levelXPMultiplier);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        UIController.instance.xpSlider.maxValue = getNextLevelXP();
        UIController.instance.xpSlider.value = currentXP;
        UIController.instance.xpText.text = currentXP.ToString() + " / " + getNextLevelXP().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);
            GameManager.EndGame();
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void GainXP() {
        // Temp
        float xpPerMob = 10f;
        currentXP += xpPerMob;

        // Level up
        if (currentXP >= getNextLevelXP()) {
            levelBaseXP = getNextLevelXP();
            currentXP = 0f;
            curLevel += 1;
            UIController.instance.lvlText.text = curLevel.ToString();
        }

        UIController.instance.xpSlider.value = currentXP;
        UIController.instance.xpText.text = currentXP.ToString() + " / " + getNextLevelXP().ToString();
    }

    public void AddWood(int amount)
    {
        wood += amount;
        UIController.instance.woodText.text = wood.ToString();
    }

    public void SubtractWood(int amount)
    {
        wood -= amount;
        UIController.instance.woodText.text = wood.ToString();
    }
}
