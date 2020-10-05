using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCanvasOnce : MonoBehaviour
{
  PlayerController pc;
  Canvas parentCanvas;

  void Start()
  {
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    pc = player.GetComponent<PlayerController>();
    pc.enabled = false;
    parentCanvas = GetComponentInParent<Canvas>();
    parentCanvas.sortingOrder = 10;
  }

  public void EnablePlayer()
  {
    pc.enabled = true;
    Destroy(this.gameObject);
    parentCanvas.sortingOrder = 0;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }
}
