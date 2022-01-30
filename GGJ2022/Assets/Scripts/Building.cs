using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;
    public int cost;

    public GameObject weapon;
    public SpriteRenderer towerSprite;
    public BoxCollider2D towerCollider;

    // Start is called before the first frame update
    void Start()
    {
        weapon.gameObject.SetActive(false);
        towerCollider = GetComponentInChildren<BoxCollider2D>();
        towerCollider.enabled = false;
    }

    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
        weapon.gameObject.SetActive(true);
        towerSprite.sortingOrder = -(int)Mathf.Round(gameObject.transform.position.y);
        towerCollider.enabled = true;
    }

    #endregion
}
