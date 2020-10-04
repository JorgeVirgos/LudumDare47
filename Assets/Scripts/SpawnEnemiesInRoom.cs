﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class SpawnEnemiesInRoom : MonoBehaviour
{

    public GameObject current_level = null;
    public GameObject PistolEnemy, RifleEnemy, ShotgunEnemy;
    public int num = 3;
     List<GameObject>  spawnpoints=new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
      
        SpawnIntoRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetSpawnPoints(GameObject c) {
        Transform t = c.transform;
         foreach (Transform tr in t)
        {
            
            if (tr.tag == "spawner")
            {
                spawnpoints.Add(tr.gameObject);
            }
        }
       
    }

    public Vector3 getRandomSpawnPoint() {
      
        return spawnpoints[UnityEngine.Random.Range(0,spawnpoints.Count)].transform.position;

    }
    void Spawn(GameObject e1, GameObject e2)
    {
        for (int i = 0; i < num; i++) {
           
            switch (UnityEngine.Random.Range(1, 3))
            {
                case 1:
                    Instantiate(e1, getRandomSpawnPoint(), Quaternion.identity);
                   
                    break;
                case 2:
                    Instantiate(e2, getRandomSpawnPoint(), Quaternion.identity);
                    break;

            }
            
               
            
        }

    }

    void SpawnIntoRoom() {
        if (current_level.name != "emptyRoom") {
            GetSpawnPoints(current_level.transform.FindChild("suelo").gameObject);
            if (current_level.transform.FindChild("suelo").tag == "shortRangeRoom")
            {
                Spawn(PistolEnemy, ShotgunEnemy);
            }
            else
            {
                Spawn(PistolEnemy, RifleEnemy);
            }
           
         }
    
    }
}
