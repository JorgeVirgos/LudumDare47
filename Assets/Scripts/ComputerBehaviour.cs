using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerBehaviour : MonoBehaviour, IInteractable
{

  public Camera targetCamera;

  // Start is called before the first frame update
  void Start()
  {
    GameObject screen = transform.Find("Screen").gameObject;
    Material mat = screen.GetComponent<MeshRenderer>().materials[0];
    RenderTexture rt = (RenderTexture)mat.GetTexture("Albedo");
    targetCamera.targetTexture = rt;
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public void Interact(GameObject interactor)
  {

  }

  IEnumerator LoadNextLevelCor()
  {
    AsyncOperation ao = SceneManager.LoadSceneAsync("AitorTest", LoadSceneMode.Additive);

    while (!ao.isDone)
    {
      yield return null;
    }
  }

  // This should be called when colliding with TriggerLoadLevel
  [ContextMenu("Load Next Level")]
  public void LoadNextLevel()
  {
    StartCoroutine(LoadNextLevelCor());
  }
}
