using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject weaponToEnable;
    public WeaponStats weaponStatsToUpgrade;
    public SpriteRenderer iconRenderer;
    public float lifespan = 10f;

    private float birthtime;

    public void Start()
    {
        birthtime = Time.time;
    }

    public void Update()
    {
        if (Time.time - birthtime > lifespan)
        {
            Debug.Log("PowerUp Expired");
            Destroy(gameObject);
        }
    }

    public void Init(Sprite icon, GameObject weaponToEnable, WeaponStats weaponStatsToUpgrade)
    {
        iconRenderer.sprite = icon;
        this.weaponToEnable = weaponToEnable;
        this.weaponStatsToUpgrade = weaponStatsToUpgrade;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            AudioManager.PowerUpPickedUp();
            if (weaponToEnable != null && !weaponToEnable.activeSelf)
            {
                weaponToEnable.SetActive(true);
            }
            else
            {
                weaponStatsToUpgrade.Upgrade();
            }
            Destroy(gameObject);
        }
    }
}
