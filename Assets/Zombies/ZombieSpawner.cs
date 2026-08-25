using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject player;
    public float spawnInterval = 5f;
    public int maxZombiesAlive = 30;
    public float minDistanceFromPlayer = 15f;

    readonly List<GameObject> spawnedZombies = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameStats.gameOver) continue;

            spawnedZombies.RemoveAll(zombie => zombie == null);

            if (spawnedZombies.Count < maxZombiesAlive)
            {
                TrySpawnZombie();
            }
        }
    }

    void TrySpawnZombie()
    {
        Terrain terrain = Terrain.activeTerrain;
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainPos = terrain.transform.position;

        float marginX = terrainSize.x * 0.1f;
        float marginZ = terrainSize.z * 0.1f;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            float x = terrainPos.x + marginX + Random.Range(0f, terrainSize.x - 2 * marginX);
            float z = terrainPos.z + marginZ + Random.Range(0f, terrainSize.z - 2 * marginZ);
            float y = terrainPos.y + terrain.SampleHeight(new Vector3(x, 0, z));
            Vector3 candidate = new Vector3(x, y, z);

            if (player != null && Vector3.Distance(candidate, player.transform.position) < minDistanceFromPlayer)
            {
                continue;
            }

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                GameObject zombie = Instantiate(zombiePrefab, hit.position, Quaternion.identity);
                spawnedZombies.Add(zombie);
                return;
            }
        }
    }
}
