using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] trees;
    public int treesToSpawn = 8;
    List<Vector3> tileWorldLocations;

    static TreeSpawner instance;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        tileWorldLocations = new List<Vector3>();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace)) {
                tileWorldLocations.Add(place);
            }
        }
    }

    public static void SpawnNewTrees() {
        if (instance != null)
            instance.StartCoroutine(instance.spawnTrees());
    }

    IEnumerator spawnTrees() {
        int treesSpawned = 0;
        while (treesSpawned < treesToSpawn) {
            // Pick random tile
            Vector3 pos = tileWorldLocations[Random.Range(0, tileWorldLocations.Count)];

            // Pick random tree
            GameObject tree = Instantiate(trees[Random.Range(0, trees.Length)]);
            tree.transform.position = pos;
            tree.transform.SetParent(transform);
            treesSpawned++;
            yield return null;
        }
    }
}
