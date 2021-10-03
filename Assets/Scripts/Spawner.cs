using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public float minSpawnDelay = 2f;
    [SerializeField] public float maxSpawnDelay = 7f;
    [SerializeField] public GameObject attackerPrefab;

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
       Instantiate(attackerPrefab, transform.position, transform.rotation);
   }

    void Update()
    {
        
    }
}
