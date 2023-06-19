using System.Collections.Generic;
using UnityEngine;

public class regionmanager : MonoBehaviour
{
    private List<GameObject> activeTiles;
    public GameObject[] tilePrefabs;

    public float tileLength = 30;
    public int numberOfTiles = 3;
    public int totalNumOfTiles = 8;

    public float zSpawn = 0;

    public Transform playerTransform;

    private int previousIndex;

    void Start()
    {
        activeTiles = new List<GameObject>();
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, totalNumOfTiles));
        }

        
    }
    void Update()
    {
        if (playerTransform.position.z - 100 >= zSpawn - (numberOfTiles * tileLength))
        {
            int index = Random.Range(0, totalNumOfTiles);

            DeleteTile();
            SpawnTile(index);
        }

    }

    public void SpawnTile(int index )
    {
        GameObject go = Instantiate(tilePrefabs[index], transform.forward * zSpawn,transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}