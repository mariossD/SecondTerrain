using UnityEngine;

public class KitSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject healthKitPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float spawnInterval = 120f;

    private GameObject[] bulletItems;
    private GameObject[] healthItems;

    private float spawnTimer = 0f;
    private bool initialized = false;

    void Update()
    {

        if (!initialized)
        {
            Initialize();
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnKitAtAllPoints();
            spawnTimer = spawnInterval;
        }
    }

    void Initialize()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            bulletItems = new GameObject[spawnPoints.Length];
            healthItems = new GameObject[spawnPoints.Length];
            initialized = true;
        }
    }

    void SpawnKitAtAllPoints()
    {

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (bulletItems[i] == null)
            {
                Vector3 bulletPos = spawnPoints[i].position + new Vector3(1.5f, 0.5f, 0);
                bulletItems[i] = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
            }

            if (healthItems[i] == null)
            {
                Vector3 healthPos = spawnPoints[i].position + new Vector3(-0.5f, 0.5f, 0);
                healthItems[i] = Instantiate(healthKitPrefab, healthPos, Quaternion.identity);
            }
        }
    }
}
