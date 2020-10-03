using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent enemy;
    private GameObject player;
    public bool Sightplayer = false;
    public float fovAngle = 160f;
    public float losRad = 20f;
    public float AttackDistance = 10.0f;
    private Weapon CurrentWeapon;
    private bool shoot;
    private float HP = 100.0f;

  public void TakeDamage(float damage) {
    HP -= damage;
    if(HP <= 0.0f) Die();
  }

  private void Die() {
    Destroy(gameObject);
  }

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        CurrentWeapon = (Weapon)this.gameObject.transform.GetChild(0).gameObject.GetComponent<Pistol>();
    }

    private void Update()
    {
        CheckView();
       
        if (Sightplayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
        if (shoot)
        {
            CurrentWeapon.Shoot();
            
        }
        else
        {
            CurrentWeapon.bIsShooting = false;
        }

    }

    void FacePlayer()
    {

    }
    void ChasePlayer()
    {
       
        enemy.destination = player.transform.position;
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        if (dist < AttackDistance)
        {
            enemy.SetDestination(transform.position);
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        
    }
    void CheckView()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Vector3.Angle(dir, transform.forward);
        if(angle < fovAngle * 0.5f)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir.normalized, out hit, losRad))
            {
                if(hit.collider.tag == "Player")
                {
                    Sightplayer = true;
                }
                else
                {
                    Sightplayer = false;
                }
            }
        }
    }
    void Patrol()
    {
        Debug.Log("patrol");
        Vector3 randompos =  Random.insideUnitSphere * 20;
        randompos += this.transform.position;
        enemy.destination = randompos;
    }
    
}
