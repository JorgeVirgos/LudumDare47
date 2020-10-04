using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
  DoorBehaviour db;

  void Start()
  {
    db = transform.parent.GetComponent<DoorBehaviour>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      db.canOpenDoor = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      db.canOpenDoor = false;
    }
  }
}
