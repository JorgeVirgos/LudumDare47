using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComputerBehaviour : MonoBehaviour, IInteractable
{
  public string nextSceneNameFirst;
  public string nextSceneNameSecond;
    public Shader shader;
  public GameObject CameraTarget;
  public float TransitionSpeed = 1.0f;
  public MeshRenderer highlightMesh;

  Scene currentScene;
  Scene nextScene;
  GameObject persitantCameraGO;
  GameObject gameMenu;
  Camera persitantCamera;
  Camera currentCamera;
  Camera nextCamera;

  GameObject Player;

  enum State
  {
    None = 0,
    SceneLoaded,
    Transitioned,
    CameraChange
  };

  State currentState;

  public void Start()
  {
    persitantCameraGO = Camera.main.gameObject;
    currentState = State.None;
    InteractableHelper.AddHighlightMaterial(highlightMesh);
  }

  public void Interact(GameObject interactor)
  {
    //InteractableHelper.ToggleHighlight(highlightMesh, false);
    InteractableHelper.RemoveHighlight(highlightMesh);
    BeginTransition(interactor);
  }

  public void SetRenderTexture()
  {
    GameObject[] go = nextScene.GetRootGameObjects();
    GameObject root = go[0];
    Player = root.transform.Find("FPSPlayer").gameObject;
    nextCamera = Player.transform.GetChild(0).GetComponent<Camera>();
    MeshRenderer mr = transform.GetChild(0).GetComponent<MeshRenderer>();
    RenderTexture rt = nextCamera.targetTexture;
    mr.material.SetTexture("_MainTex", rt);
    // Disable Movement
    Player.GetComponent<PlayerController>().enabled = false;
    Transform t = Player.transform.GetChild(0);
    t.GetChild(t.childCount - 1).gameObject.GetComponent<Canvas>().sortingOrder = -1;
  }

  IEnumerator LoadNextLevelCor()
  {
    persitantCamera = persitantCameraGO.GetComponent<Camera>();
    currentCamera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();

    
    if (SceneManager.GetActiveScene().name == nextSceneNameFirst)
        nextSceneNameFirst = nextSceneNameSecond;

    AsyncOperation ao = SceneManager.LoadSceneAsync(nextSceneNameFirst, LoadSceneMode.Additive);

    Debug.Log("Loading Next Scene: " + nextSceneNameFirst);

    while (!ao.isDone)
    {
      yield return null;
    }

    currentState = State.SceneLoaded;

    currentScene = SceneManager.GetActiveScene();
    nextScene = SceneManager.GetSceneByName(nextSceneNameFirst);
    SetRenderTexture(); 
  }

  IEnumerator BeginTransitionCor(GameObject player)
  {
    if (currentState != State.SceneLoaded)
    {
      Debug.LogError("No level loaded.");
      yield break;
    }
    currentState = State.Transitioned;
    // Disable Player Controller
    player.GetComponent<PlayerController>().enabled = false;
    Transform t = player.transform.GetChild(0);
    t.GetChild(t.childCount - 1).gameObject.GetComponent<Canvas>().sortingOrder = -1;
    // Set cursor visible
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    Vector3 startPos = currentCamera.transform.position;
    Vector3 endPos = CameraTarget.transform.position;

    Quaternion startQuat = currentCamera.transform.rotation;
    Quaternion endQuat = CameraTarget.transform.rotation;

    float alpha = 0.0f;

    while (alpha <= 1.0f)
    {
      currentCamera.transform.position = Vector3.Lerp(startPos, endPos, alpha);
      currentCamera.transform.rotation = Quaternion.Lerp(startQuat, endQuat, alpha);
      alpha += TransitionSpeed * Time.deltaTime;
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
    currentState = State.CameraChange;

    gameMenu.SetActive(false);

    // Enable Movement
    Player.GetComponent<PlayerController>().enabled = true;
    Transform t = Player.transform.GetChild(0);
    t.GetChild(t.childCount - 1).gameObject.GetComponent<Canvas>().sortingOrder = 1;

    SceneManager.SetActiveScene(nextScene);

    GameObject a = persitantCamera.transform.GetChild(0).transform.GetChild(0).gameObject;
    Material mat = a.GetComponent<Image>().material = new Material(shader);
    mat.mainTexture = nextCamera.targetTexture;

    // Set cursor invisible
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    SceneManager.UnloadSceneAsync(currentScene);
    yield return null;
  }

  public void LoadNextLevel()
  {
    StartCoroutine(LoadNextLevelCor());
  }

  public void BeginTransition(GameObject player)
  {
    StartCoroutine(BeginTransitionCor(player));
  }

  public void ChangeCamera()
  {
    StartCoroutine(ChangeCameraCor());
  }

  public void SetHighlightActive(bool active)
  {
    InteractableHelper.ToggleHighlight(highlightMesh, active);
  }
}
