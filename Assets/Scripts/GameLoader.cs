using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
  public string FirstLevel;

  void Awake()
  {
    SceneManager.LoadSceneAsync(FirstLevel);
    Destroy(this.gameObject);
  }
}
