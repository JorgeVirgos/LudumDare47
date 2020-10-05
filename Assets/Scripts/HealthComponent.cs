using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour {

  public float MaxHealth = 100.0f;
  public float MaxArmor = 100.0f;

  public RectTransform HealthBar;
  public RectTransform ArmourBar;
  
  private float CurrentHealth;
  private float CurrentArmor;
  
  // Start is called before the first frame update
  void Start() {
    CurrentHealth = MaxHealth;
    CurrentArmor = MaxArmor;
  }

  public void TakeDamage(float dmg) {
    if(CurrentArmor > 0.0f) {
      CurrentArmor -= dmg;
      if(ArmourBar) {
        ArmourBar.offsetMax -= new Vector2(dmg * 2.0f, 0.0f);
      }
    } else {
      CurrentHealth -= dmg;
      if(HealthBar) {
        HealthBar.offsetMax -= new Vector2(dmg * 2.0f, 0.0f);
      }
      if(CurrentHealth <= 0.0f) {
        Die();
      }
    }
  }

  void Die() {
    if(gameObject.tag == "Player") {
            // Load Scene
            GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
            mc.SetActive(false);
            // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
            SceneManager.LoadScene("Sorpresa");
            return;
    }
    Destroy(gameObject);
  }

  public void Heal(float hp) {
    CurrentHealth += hp;
    if(HealthBar) {
        HealthBar.offsetMax += new Vector2(hp * 2.0f, 0.0f);
        if(HealthBar.offsetMax.x > 0.0f) HealthBar.offsetMax = new Vector2(0.0f, 0.0f);
    }
    if(CurrentHealth > MaxHealth) {
      CurrentHealth = MaxHealth;
    }
  }

  public void RestoreArmor(float hp) {
    CurrentArmor += hp;
    if(ArmourBar) {
      ArmourBar.offsetMax += new Vector2(hp * 2.0f, 0.0f);
      if(ArmourBar.offsetMax.x > 0.0f) ArmourBar.offsetMax = new Vector2(0.0f, 0.0f);
    }
    if(CurrentArmor > MaxArmor) {
      CurrentArmor = MaxArmor;
    }
  }
}
