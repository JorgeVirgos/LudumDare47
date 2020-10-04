using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
  public string FirstLevel;

  void Awake()
  {
    StartCoroutine(WhyAreWeStillHere());
  }

  IEnumerator WhyAreWeStillHere()
  {
    AsyncOperation ao = SceneManager.LoadSceneAsync(FirstLevel, LoadSceneMode.Additive);

    while (!ao.isDone)
    {
      yield return null;
    }

    Scene s = SceneManager.GetSceneByName(FirstLevel);
    SceneManager.SetActiveScene(s);

    Destroy(this.gameObject);
  }
}
