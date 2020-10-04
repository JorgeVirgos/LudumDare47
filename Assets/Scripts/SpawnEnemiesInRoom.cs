using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesInRoom : MonoBehaviour
{

    public GameObject current_level = null;
    public GameObject PistolEnemy, RifleEnemy, ShotgunEnemy;
    public int num = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindObjectwithTag(string _tag)
    {
        actors.Clear();
        Transform parent = transform;
        GetChildObject(parent, _tag);
    }
   
    void SpawnIntoRoom() {
        if (current_level.name != "emptyRoom") {
            GameObject[] spawnpoints;
            spawnpoints=GameObject.FindGameObjectsWithTag("spawner");
            if (current_level.transform.FindChild("suelo").tag == "shortRange") {
                for (int i = 0; i < spawnpoints.Length; i++) {
                    GameObject enemyToInst;
                    switch (Random.Range(1, 3))
                    {
                        case 1:
                            enemyToInst = PistolEnemy;
                         break;
                        case 2:
                            enemyToInst = ShotgunEnemy;
                        break;

                    }  
                    Instantiate(enemyToInst[UnityEngine.Random.Range(0, enemy.Length - 1)], transform.position, Quaternion.identity);
                }
    
            }
            else
            {

            }
           
         }
    
    }
}
