using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SAGSpawner : MonoBehaviour
{
    public GameObject proyectile;
    public int num;
    // Start is called before the first frame update

    public Vector3 ImpulseDirection;
    public float Impulse;
     float aliveTime=2f;
    public float Damage = 10.0f;

    private Rigidbody Rb;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.AddForce(ImpulseDirection * Impulse, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (aliveTime < 1.8f)
        {
            Destroy(gameObject);
        }
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
