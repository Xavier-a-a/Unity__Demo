using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject newEnemy;
    private float spawnTimer;
    public float interval;
    public float range;

    void SpawnNewEnemy()
    {
        float rad = Random.Range(0f, range);
        float angle = Random.Range(0f, 2f) * Mathf.PI;
        float x = rad * Mathf.Cos(angle) + this.transform.position.x;
        float y = this.transform.position.y;
        float z = rad * Mathf.Sin(angle) + this.transform.position.z;
        Instantiate(newEnemy, new Vector3(x, y, z), Quaternion.identity);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = interval;
    }

    
    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            SpawnNewEnemy();
            spawnTimer = interval;
        }
    }
}
