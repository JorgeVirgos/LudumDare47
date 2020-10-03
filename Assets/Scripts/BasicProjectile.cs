using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour {

  public Vector3 ImpulseDirection;
  public float Impulse;
  public float aliveTime;
  public float Damage = 10.0f;

  private Rigidbody Rb;

  // Start is called before the first frame update
  void Start() {
    Rb = GetComponent<Rigidbody>();
    Rb.AddForce(ImpulseDirection * Impulse, ForceMode.VelocityChange);
  }

  // Update is called once per frame
  void Update() {
    aliveTime -= Time.deltaTime;
    if(aliveTime <= 0.0f) {
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter(Collision collision) {
    if(collision.gameObject.tag == "Projectile") return;
    if(collision.gameObject.tag == "Enemy") {
      EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
      if(enemy) {
        enemy.TakeDamage(Damage);
      }
    }
    Destroy(gameObject);
  }
}
