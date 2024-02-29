using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPrefab : MonoBehaviour
{
    public List<GameObject> modelPrefabs;
    public Terrain terrain;
    public int minMonsters;
    public int maxMonsters;

    public List<GameObject> treePrefabs;

    public void Start()
    {
        SpawnRandomMonstersOnTerrain(minMonsters, maxMonsters);
        SpawnRandomTreesOnTerrain(minMonsters, maxMonsters);
    }

    void SpawnRandomMonstersOnTerrain(int minMonsters, int maxMonsters)
    {
        TerrainCollider terrainCollider = terrain.GetComponent<TerrainCollider>();

        if (terrainCollider != null && modelPrefabs.Count > 0)
        {
            float terrainWidth = terrainCollider.bounds.size.x;
            float terrainLength = terrainCollider.bounds.size.z;

            int numberOfMonsters = Random.Range(minMonsters, maxMonsters + 1);

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int randomIndex = Random.Range(0, modelPrefabs.Count);
                GameObject selectedPrefab = modelPrefabs[randomIndex];

                float x = Random.Range(terrainCollider.bounds.min.x, terrainCollider.bounds.min.x + terrainWidth);
                float z = Random.Range(terrainCollider.bounds.min.z, terrainCollider.bounds.min.z + terrainLength);

                float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;

                Vector3 randomSpawnPosition = new Vector3(x, y, z);
                Instantiate(selectedPrefab, randomSpawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Terrain Collider or Monster Prefabs list is not properly set up.");
        }
    }

    void SpawnRandomTreesOnTerrain(int minMonsters, int maxMonsters)
    {
        TerrainCollider terrainCollider = terrain.GetComponent<TerrainCollider>();

        if (terrainCollider != null && treePrefabs.Count > 0)
        {
            float terrainWidth = terrainCollider.bounds.size.x;
            float terrainLength = terrainCollider.bounds.size.z;

            int numberOfMonsters = Random.Range(minMonsters, maxMonsters + 1);

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int randomIndex = Random.Range(0, treePrefabs.Count); // Choose a random monster prefab from the list
                GameObject selectedPrefab = treePrefabs[randomIndex];

                float x = Random.Range(terrainCollider.bounds.min.x, terrainCollider.bounds.min.x + terrainWidth);
                float z = Random.Range(terrainCollider.bounds.min.z, terrainCollider.bounds.min.z + terrainLength);

                float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;

                Vector3 randomSpawnPosition = new Vector3(x, y, z);
                Instantiate(selectedPrefab, randomSpawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Terrain Collider or Monster Prefabs list is not properly set up.");
        }
    }
}
