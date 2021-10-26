using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 6f;
    public int maxEnemies = 3;

    private float spawnTimer = 0;
    private float waitTimer = 0;
    private float spawnAt = 0; //random spawn time
    private float waitFor = 0; //random wait value
    private bool spawning;
    private int enemyCounter;

    void Start()
    {
        PickRandomTimeToSpawn();
    }

    void Update()
    {
        if (spawning)
        {
            //spawning new enemy
            if (spawnTimer < spawnAt)
            {
                spawnTimer += Time.deltaTime;
            }
            else
            {
                Spawn();
            }
        }
        else
        {
            //waiting
            if(waitTimer < waitFor)
            {
                waitTimer += Time.deltaTime;
            }
            else
            {
                PickRandomTimeToSpawn();
            }
        }
    }

    void PickRandomTimeToSpawn()
    {
        spawnAt = Random.Range(minSpawnDelay, maxSpawnDelay);
        waitFor = Random.Range(minSpawnDelay, maxSpawnDelay);
        spawning = true;
        waitTimer = 0;
    }

    void Spawn()
    {
        if (enemyCounter < maxEnemies)
        {
            Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation);
            spawning = false;
            enemyCounter += 1;
        }
    }

    public void OnEnemyKilled()
    {
        if (enemyCounter > 0)
        {
            enemyCounter -= 1;
        }
    }
}
