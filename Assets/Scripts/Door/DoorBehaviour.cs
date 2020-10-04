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

  private Vector3 startPos;
  [HideInInspector]
  public bool canOpenDoor;
  [HideInInspector]
  public bool isOpen;
  [HideInInspector]
  public bool transitioning;
  [HideInInspector]
  public bool shouldDestroy;

  void Start()
  {
    startPos = doorMesh.transform.position;
    canOpenDoor = false;
    transitioning = false;
    isOpen = false;
    shouldDestroy = false;
  }

  void Update()
  {
    // if (neededInteractable == null)
    {
      bool shouldOpen = canOpenDoor && !isOpen;

      if (shouldOpen)
        OpenDoor();

      bool shouldClose = !canOpenDoor && isOpen;
      if (shouldClose)
        CloseDoor(false);
    }

    if (shouldDestroy)
    {
      Destroy(this);
    }
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
    yield break;
  }

  public void OpenDoor()
  {
    if (transitioning) return;
    StartCoroutine(TranslateDoorCor(startPos, target.position, openSpeed, OpenDoorCor()));
  }

  private void CloseDoor(bool destroy)
  {
    if (transitioning) return;
    StartCoroutine(TranslateDoorCor(target.position, startPos, openSpeed, CloseDoorCor(destroy)));
  }
  
  void IInteractable.Interact(GameObject interactor)
  {
    // TODO:
    
  }
}
