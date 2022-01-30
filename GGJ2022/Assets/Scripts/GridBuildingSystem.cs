using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public GameObject wrenchIcon;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    public bool tempToggleEnabled = true;
    public bool isBuilding = false;

    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if(!temp.Placed && !isBuilding)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && temp)
        {
            if (temp.CanBePlaced() && PlayerInfo.instance.wood >= temp.cost)
            {
                StartCoroutine(BuildBuilding(3f, temp.cost));
            }
            else if (PlayerInfo.instance.wood < temp.cost)
            {
                Debug.Log("You need more wood!");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            temp.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Tilemap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    public void ToggleTemp()
    {
        if (temp)
        {
            temp.gameObject.SetActive(!temp.gameObject.activeInHierarchy);
            temp.gameObject.transform.position = (new Vector3(1000, 1000, 1));
        }
    }

    public void InitializeWithBuilding(GameObject building)
    {
        ClearArea();
        if (!temp)
        {
            temp = Instantiate(building, new Vector3(1000, 1000, 1), Quaternion.identity).GetComponent<Building>();
            FollowBuilding();
        } else if (temp.Placed)
        {
            temp = Instantiate(building, new Vector3(1000, 1000, 1), Quaternion.identity).GetComponent<Building>();
            FollowBuilding();
        }
    }
    
    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot Place Here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    IEnumerator BuildBuilding(float buildTime, int woodCost)
    {
        tempToggleEnabled = false;
        PlayerController.instance.ToggleBuildMode();
        isBuilding = true;
        PlayerInfo.instance.SubtractWood(woodCost);
        GameObject wrench = Instantiate(wrenchIcon, new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z), Quaternion.identity);
        var col = temp.GetComponentInChildren<SpriteRenderer>().color;
        float currentTime = 0f;
        while (currentTime < buildTime)
        {
            currentTime += Time.deltaTime;
            col.a = currentTime / buildTime;
            temp.GetComponentInChildren<SpriteRenderer>().color = col;
            yield return null;
        }
        yield return null;
        temp.Place();
        isBuilding = false;
        temp = Instantiate(temp, new Vector3(1000, 1000, 1), Quaternion.identity).GetComponent<Building>();
        Destroy(wrench);
        temp.gameObject.SetActive(false);
        tempToggleEnabled = true;
    }

    #endregion
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}