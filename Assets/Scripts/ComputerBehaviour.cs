using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComputerBehaviour : MonoBehaviour, IInteractable
{
  public string nextSceneName;
  public Shader shader;
  public GameObject CameraTarget;
  public float TransitionSpeed = 1.0f;

  Scene currentScene;
  Scene nextScene;
  GameObject persitantCameraGO;
  GameObject gameMenu;
  Camera persitantCamera;
  Camera currentCamera;
  Camera nextCamera;

  public void Start()
  {
    persitantCameraGO = Camera.main.gameObject;
  }

  public void Interact(GameObject interactor)
  {

  }

  public void SetRenderTexture()
  {
    GameObject[] go = nextScene.GetRootGameObjects();
    nextCamera = go[0].transform.Find("Camera").gameObject.GetComponent<Camera>();
    MeshRenderer mr = transform.GetChild(0).GetComponent<MeshRenderer>();
    RenderTexture rt = nextCamera.targetTexture;
    mr.material.SetTexture("_MainTex", rt);
  }

  IEnumerator LoadNextLevelCor()
  {
    persitantCamera = persitantCameraGO.GetComponent<Camera>();
    currentCamera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();

    AsyncOperation ao = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

    while (!ao.isDone)
    {
      yield return null;
    }

    currentScene = SceneManager.GetActiveScene();
    nextScene = SceneManager.GetSceneByName(nextSceneName);
    SetRenderTexture(); 
  }

  IEnumerator BeginTransitionCor(float Speed)
  {
    Vector3 startPos = currentCamera.transform.position;
    Vector3 endPos = CameraTarget.transform.position;

    Quaternion startQuat = currentCamera.transform.rotation;
    Quaternion endQuat = CameraTarget.transform.rotation;

    float alpha = 0.0f;

    while (alpha <= 1.0f)
    {
      currentCamera.transform.position = Vector3.Lerp(startPos, endPos, alpha);
      currentCamera.transform.rotation = Quaternion.Lerp(startQuat, endQuat, alpha);
      alpha += Speed * Time.deltaTime;
      yield return null;
    }

    currentCamera.transform.position = endPos;
    currentCamera.transform.rotation = endQuat;

    // Show Menu
    yield return new WaitForSeconds(0.2f);

    gameMenu = persitantCameraGO.transform.GetChild(0).GetChild(1).gameObject;
    gameMenu.SetActive(true);

    Button b = gameMenu.transform.GetChild(0).GetComponent<Button>();
    b.onClick.RemoveAllListeners();
    b.onClick.AddListener(ChangeCamera);
  }

  IEnumerator ChangeCameraCor()
  {
    gameMenu.SetActive(false);

    SceneManager.SetActiveScene(nextScene);

    GameObject a = persitantCamera.transform.GetChild(0).transform.GetChild(0).gameObject;
    Material mat = a.GetComponent<Image>().material = new Material(shader);
    mat.mainTexture = nextCamera.targetTexture;

    yield return new WaitForSeconds(1.0f);

    SceneManager.UnloadSceneAsync(currentScene);
  }

  [ContextMenu("Load Next Level")]
  public void LoadNextLevel()
  {
    StartCoroutine(LoadNextLevelCor());
  }

  [ContextMenu("Begin Transition")]
  public void BeginTransition()
  {
    StartCoroutine(BeginTransitionCor(TransitionSpeed));
  }

  [ContextMenu("Change Camera")]
  public void ChangeCamera()
  {
    StartCoroutine(ChangeCameraCor());
  }

}
