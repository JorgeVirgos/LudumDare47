using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour {

  public float MaxHealth = 100.0f;
  public float MaxArmor = 100.0f;
  
  private float CurrentHealth;
  private float CurrentArmor;
  
  // Start is called before the first frame update
  void Start() {
    CurrentHealth = MaxHealth;
  }

  public void TakeDamage(float dmg) {
    if(CurrentArmor > 0.0f) {
      CurrentArmor -= dmg;
    } else {
      CurrentHealth -= dmg;
      if(CurrentHealth <= 0.0f) {
        Die();
      }
    }
  }

  void Die() {
    Destroy(gameObject);
  }

  public void Heal(float hp) {
    CurrentHealth += hp;
    if(CurrentHealth > MaxHealth) {
      CurrentHealth = MaxHealth;
    }
  }

  public void RestoreArmor(float hp) {
    CurrentArmor += hp;
    if(CurrentArmor > MaxArmor) {
      CurrentArmor = MaxArmor;
    }
  }
}
