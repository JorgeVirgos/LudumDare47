using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class SAGProyectiles : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer=10;
    //NavMeshAgent proyectile;
    private GameObject enemy;
    public float speed = 1;
   
    private bool chase = false;
    private void Die()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
       // proyectile = GetComponent<NavMeshAgent>();
      
        
    }

    private void Update()
    {
        if (timer > 0)
        {

            if (chase && enemy != null)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, step);
          
            }
            else {
                FindClosestEnemy();
            }
            timer -= Time.deltaTime;

        }
        else 
        {
            if (timer <= 0)
            {
               Die();
               
            }
        }

    }

    public void FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
       
        chase = true;
        enemy= closest;
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Untagged") return;
        UnityEngine.Debug.Log(col.gameObject.tag);
        if (col.gameObject.CompareTag("Enemy")) {
            UnityEngine.Debug.Log("is");
            HealthComponent enemy = col.gameObject.GetComponent<HealthComponent>();
            if (enemy)
            {
                UnityEngine.Debug.Log("enemy");
                enemy.TakeDamage(1000) ;
            }
            Die();
        }
    }
    void ChaseEnemy()
    {/*
         proyectile.destination = enemy.transform.position;
        float dist = Vector3.Distance(enemy.transform.position, this.transform.position);
        startTimer = true;
        */
       
    }
    
}
