using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      this.transform.parent.GetComponent<ComputerBehaviour>().LoadNextLevel();
      Destroy(this);
    }
  }
}
