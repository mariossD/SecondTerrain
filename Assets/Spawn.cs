using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int number;
    public float spawnRadius;
    public bool spawnOnStart = true;
    void Start()
    {
        if (spawnOnStart)
        {
            SpawnZombie();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!spawnOnStart && other.gameObject.CompareTag("Player"))
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 10.0f, NavMesh.AllAreas))
            {
                Instantiate(zombiePrefab, hit.position, Quaternion.identity);
            }
            else
                i--;
        }
    }
}
