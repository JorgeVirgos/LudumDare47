﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
  GameObject parent;
  Transform cameraTransform;

  bool shouldInteract;

  LineDrawer lineDrawer;

  void Start()
  {
    parent = transform.parent.gameObject;
    cameraTransform = transform.GetChild(0);
    shouldInteract = false;
    lineDrawer = new LineDrawer(this.transform, 0.02f);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      shouldInteract = true;
    }
    if (Input.GetKeyUp(KeyCode.E))
    {
      shouldInteract = false;
    }
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << LayerMask.NameToLayer("Interactable"); 

    RaycastHit hit;
    // Does the ray intersect any objects excluding the player layer
    Vector3 initPos = cameraTransform.position;
    initPos.y -= 0.1f;
    Vector3 dir = cameraTransform.transform.TransformDirection(Vector3.forward);
    float distance = 4.0f;
    if (Physics.Raycast(initPos, dir, out hit, distance, layerMask))
    {
      lineDrawer.DrawLineInGameView(initPos, initPos + (dir * distance), Color.green);
      if (shouldInteract)
      {
        shouldInteract = false;
        GameObject go = hit.collider.gameObject;
        IInteractable interactable = go.GetComponent<IInteractable>();
        interactable.Interact(this.gameObject);
      }
    } else
    {
      lineDrawer.DrawLineInGameView(initPos, initPos + (dir * distance), Color.red);
    }
  }
}
