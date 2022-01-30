using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponUIIcon : MonoBehaviour
{
    public WeaponStats weaponStats;
    public TextMeshProUGUI text;

    public void Update()
    {
        text.text = weaponStats.currentProfile.ToString();
    }
}
