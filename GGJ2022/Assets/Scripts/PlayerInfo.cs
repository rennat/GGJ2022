using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

    public float playerMineSpeed = 1f;
    public float currentHealth;
    public int maxHealth;
    public int wood = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
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

    public void AddWood(int amount)
    {
        wood += amount;
        UIController.instance.woodText.text = wood.ToString();
    }
}
