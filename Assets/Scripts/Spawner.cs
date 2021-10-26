using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 3;
    [SerializeField] public float minSpawnDelay = 2f;
    [SerializeField] public float maxSpawnDelay = 7f;
    [SerializeField] public GameObject attackerPrefab;

    int enemyCounter;
    bool spawn = true;
    
    IEnumerator Start()
    {
        while(spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnAttacker();
        }
    }


   private void SpawnAttacker()
   {
        if (enemyCounter < maxEnemies)
        {
            Instantiate(attackerPrefab, transform.position, transform.rotation);
            enemyCounter += 1;
        }
   }

    void Update()
    {
        
    }

    public void OnEnemyKilled()
    {
        if(enemyCounter > 0)
        {
            enemyCounter -= 1;
        }
    }
}
