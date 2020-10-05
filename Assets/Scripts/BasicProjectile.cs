using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour {

  public Vector3 ImpulseDirection;
  public float Impulse;
  public float aliveTime;
  public float Damage = 10.0f;
  public AudioClip[] bulletClips;

  private Rigidbody Rb;
  private AudioSource SoundSource;

  // Start is called before the first frame update
  void Start() {
    Rb = GetComponent<Rigidbody>();
    Rb.AddForce(ImpulseDirection * Impulse, ForceMode.VelocityChange);

    SoundSource = GetComponent<AudioSource>();
    if(!SoundSource) {
      SoundSource = gameObject.AddComponent<AudioSource>();
      SoundSource.playOnAwake = false;
    }
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
      HealthComponent character = collision.gameObject.GetComponent<HealthComponent>();
      if(character) {
        character.TakeDamage(Damage);
      }
    }
    int rand = Random.Range(0, bulletClips.Length - 1);
    SoundSource.PlayOneShot(bulletClips[rand], 0.2f);
    gameObject.GetComponent<BoxCollider>().enabled = false;
    //gameObject.GetComponent<MeshRenderer>().enabled = false;
    gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
  }
}
