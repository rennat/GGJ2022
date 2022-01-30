using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats.asset", menuName = "WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [System.Serializable]
    public class Profile
    {
        public float range = 10;
        public float rateOfFire = 1;
        public float projectileDamage = 1;
        public float projectileSpeed = 1;
        public float projectileLifespan = 3;
        public float projectileCount = 1;
        public float projectileHoming = 0;
        public float aoeRadius = 0;
        public float aoeLimit = 0;
        public float weaponSize = 1;
        public float weaponSpeed = 1;
        public float cooldown = 0;
    }

    public int currentProfile = 0;
    public List<Profile> profiles = new List<Profile>();

    public Profile CurrentProfile => profiles[currentProfile];

    public float Range => CurrentProfile.range;
    public float RateOfFire => CurrentProfile.rateOfFire;
    public float ProjectileDamage => CurrentProfile.projectileDamage;
    public float ProjectileSpeed => CurrentProfile.projectileSpeed;
    public float ProjectileLifespan => CurrentProfile.projectileLifespan;
    public float ProjectileCount => CurrentProfile.projectileCount;
    public float ProjectileHoming => CurrentProfile.projectileHoming;
    public float AoeRadius => CurrentProfile.aoeRadius;
    public float AoeLimit => CurrentProfile.aoeLimit;
    public float WeaponSize => CurrentProfile.weaponSize;
    public float WeaponSpeed => CurrentProfile.weaponSpeed;
    public float Cooldown => CurrentProfile.cooldown;

    public float Upgrade() => currentProfile = Mathf.Min(currentProfile + 1, profiles.Count - 1);
}
