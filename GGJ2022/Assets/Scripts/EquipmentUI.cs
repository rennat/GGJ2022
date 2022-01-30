using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject umbrellaGun;
    public GameObject umbrellaGunTile;

    public GameObject umbrellaBomb;
    public GameObject umbrellaBombTile;

    public GameObject umbrellaAura;
    public GameObject umbrellaAuraTile;

    public void Update()
    {
        umbrellaGunTile.SetActive(umbrellaGun.activeInHierarchy);
        umbrellaBombTile.SetActive(umbrellaBomb.activeInHierarchy);
        umbrellaAuraTile.SetActive(umbrellaAura.activeInHierarchy);
    }

}
