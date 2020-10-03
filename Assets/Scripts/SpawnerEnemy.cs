using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    public GameObject[] enemy;
    public float spawnTime = 6f;
 
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        Instantiate(enemy[UnityEngine.Random.Range(0, enemy.Length - 1)], transform.position, Quaternion.identity);
    }
}
