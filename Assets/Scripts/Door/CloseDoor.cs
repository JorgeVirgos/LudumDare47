using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
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
      db.shouldDestroy = true;
      Destroy(this);
    }
  }
}
