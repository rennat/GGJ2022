using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpTrack
    {
        public Sprite icon;
        public GameObject weapon;
        public WeaponStats weaponStats;
        public PowerUp prefab;
    }

    public static PlayerInfo instance;
    public delegate void PlayerTakesDamageDelegate(float amount);
    public static PlayerTakesDamageDelegate OnPlayerTakesDamage;

    public float playerMineSpeed = 1f;
    public float currentHealth;
    public float currentXP = 0f;
    public int maxHealth;
    public int wood = 0;
    public float levelBaseXP = 500f;
    public float levelXPMultiplier = 1.2f;
    public PowerUpTrack[] powerUps;

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

        foreach (var powerUpTrack in powerUps)
        {
            powerUpTrack.weaponStats.currentProfile = 0;
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
        AudioManager.PlayerTakesDamage();
        OnPlayerTakesDamage?.Invoke(amount);
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
            LevelUp();
        }

        UIController.instance.xpSlider.value = currentXP;
        UIController.instance.xpText.text = currentXP.ToString() + " / " + getNextLevelXP().ToString();
    }

    void LevelUp() {
        levelBaseXP = getNextLevelXP();
        currentXP = 0f;
        curLevel += 1;
        UIController.instance.lvlText.text = curLevel.ToString();

        // Enable random power ups
        var powerUpsToSpawn = powerUps
            .OrderBy(x => Random.Range(0f, 1f))
            .Where(x => (x.weapon != null && !x.weapon.activeInHierarchy) || x.weaponStats.currentProfile < x.weaponStats.profiles.Count() - 1)
            .Take(2);
        foreach (var powerUpTrack in powerUpsToSpawn)
        {
            var point = Random.insideUnitCircle * 4;
            var powerup = Instantiate(powerUpTrack.prefab, new Vector3(point.x, point.y, 0), Quaternion.identity).GetComponent<PowerUp>();
            powerup.Init(powerUpTrack.icon, powerUpTrack.weapon, powerUpTrack.weaponStats);
        }
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
