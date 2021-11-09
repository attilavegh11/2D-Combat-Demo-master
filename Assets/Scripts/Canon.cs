using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject canonHand;
    public float repeatEverySeconds = 3f;
    internal GameObject enemyInfront;

    void Start()
    {
        InvokeRepeating("Shoot", 0, repeatEverySeconds);
    }

    void Update()
    {
        
    }

    public void Shoot()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, canonHand.transform.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().enemyInfront = enemyInfront;

    }
}
