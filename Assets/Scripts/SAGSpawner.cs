using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SAGSpawner : MonoBehaviour
{
    public GameObject proyectile;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDestroy()
    {
      SpawnProyectiles(num);  
    }
    void SpawnProyectiles(int I) {
        for (int i = 0; i < I; i++)
           
        {
            Instantiate(proyectile, (new Vector3(i * Random.Range(0.1f, 0.5f), i * Random.Range(0.1f, 0.5f), i * Random.Range(0.1f, 0.5f)) + gameObject.transform.position) , Quaternion.identity);
        }
    }
}
