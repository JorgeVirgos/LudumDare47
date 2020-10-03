using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
  GameObject parent;
  Transform cameraTransform;

  bool shouldInteract;

  void Start()
  {
    parent = transform.parent.gameObject;
    cameraTransform = transform.GetChild(0);
    shouldInteract = false;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      shouldInteract = true;
    } else
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
    Vector3 dir = cameraTransform.transform.TransformDirection(Vector3.forward);
    Vector3 initPos = cameraTransform.position; 
    if (Physics.Raycast(initPos, dir, out hit, Mathf.Infinity, layerMask))
    {
      if (shouldInteract)
      {
        GameObject go = hit.collider.gameObject;
        IInteractable interactable = go.GetComponent<IInteractable>();
        interactable.Interact(this.gameObject);
        shouldInteract = false;
      }
    }
  }
}
