using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class SpawnEnemiesInRoom : MonoBehaviour
{

    /*public GameObject current_level = null;
    public GameObject PistolEnemy, RifleEnemy, ShotgunEnemy;
    public int num = 3;
   
    // Start is called before the first frame update
    void Start()
    {
        SpawnIntoRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   


    public Vector3 getRandomSpawnPoint() {
        GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("spawner");
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform.position;

    }
    void Spawn(GameObject e1, GameObject e2)
    {
        for (int i = 0; i < num; i++) {
            GameObject enemy=new GameObject();
            switch (Random.Range(1, 3))
            {
                case 1:
                    enemy = e1;
                    break;
                case 2:
                    enemy = e2;
                    break;

            }
            Instantiate(enemy, getRandomSpawnPoint(), Quaternion.identity); 
        }

    }

    void SpawnIntoRoom() {
        if (current_level.name != "emptyRoom") {
            GameObject[] spawnpoints;
            if (current_level.transform.FindChild("suelo").tag == "shortRangeRoom")
            {
                Spawn(PistolEnemy, ShotgunEnemy);
            }
            else
            {
                Spawn(PistolEnemy, RifleEnemy);
            }
           
         }
    
    }*/
}
