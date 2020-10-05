using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour, IInteractable
{
  public float openSpeed = 1.5f;
  public float closeSpeed = 2.25f;
  public Transform target;
  public Transform doorMesh;
  public BoxCollider openTrigger;
  public BoxCollider closeTrigger;
  public PickableObject.KeyNumber RequiredKey = PickableObject.KeyNumber.kKeyNumberNone;

  private Vector3 startPos;
  [HideInInspector]
  public bool canOpenDoor;
  [HideInInspector]
  public bool isOpen;
  [HideInInspector]
  public bool transitioning;
  [HideInInspector]
  public bool shouldDestroy;

  bool HasKey;

  void Start()
  {
    startPos = doorMesh.transform.position;
    canOpenDoor = false;
    transitioning = false;
    isOpen = false;
    shouldDestroy = false;
    InteractableHelper.AddHighlightMaterial(doorMesh.GetComponent<MeshRenderer>());
    HasKey = RequiredKey != PickableObject.KeyNumber.kKeyNumberNone;
    if (HasKey)
    {
      Color color = PickableObject.GetKeyColor(RequiredKey);
      Material mat = doorMesh.GetComponent<MeshRenderer>().materials[0];
      mat.color = color;
    }
    
  }

  void Update()
  {
    if (!HasKey)
    {
      bool shouldOpen = canOpenDoor && !isOpen;

      if (shouldOpen)
        OpenDoor();

    }
    bool shouldClose = !canOpenDoor && isOpen;

    if (shouldClose)
      CloseDoor(shouldDestroy);

    
  }

  IEnumerator TranslateDoorCor(Vector3 init, Vector3 end, float speed, IEnumerator onFinish)
  {
    transitioning = true;

    float alpha = 0.0f;

    while (alpha <= 1.0f)
    {
      doorMesh.position = Vector3.Lerp(init, end, alpha);
      alpha += speed * Time.deltaTime;
      yield return null;
    }

    doorMesh.transform.position = end;

    transitioning = false;
    StartCoroutine(onFinish);
    yield break;
  }

  IEnumerator Empty()
  {
    yield break;
  }

  IEnumerator OpenDoorCor()
  {
    isOpen = true;
    yield break;
  }

  IEnumerator CloseDoorCor(bool destroy)
  {
    isOpen = false;
    //if (destroy)
    //  Destroy(this);
    yield break;
  }

  public void OpenDoor()
  {
    Debug.Log("OpenDoorStart");
    if (transitioning) return;
    Debug.Log("OpenDoorEnd");
    StartCoroutine(TranslateDoorCor(startPos, target.position, openSpeed, OpenDoorCor()));
  }

  private void CloseDoor(bool destroy)
  {
    Debug.Log("CloseDoorStart");
    if (transitioning) return;
    Debug.Log("CloseDoorEnd");
    StartCoroutine(TranslateDoorCor(target.position, startPos, openSpeed, CloseDoorCor(destroy)));
  }
  
  void IInteractable.Interact(GameObject interactor)
  {
    GameObject go = GameObject.Find("Inventory");
    InventorySystem iSys = go.GetComponent<InventorySystem>();
    if (iSys.HasThisKey(RequiredKey))
    {
      OpenDoor();
    }
  }

  public void SetHighlightActive(bool active)
  {
    InteractableHelper.ToggleHighlight(doorMesh.GetComponent<MeshRenderer>(), active);
  }
}
