using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
  void Interact(GameObject interactor);
  void SetHighlightActive(bool active);
}

public class InteractableHelper
{
  static Material mat;
  public static void AddHighlightMaterial(Renderer renderer)
  {
    if (mat == null)
      mat = Resources.Load("CustomOutlineMat", typeof(Material)) as Material;

    List<Material> mats = new List<Material>(renderer.materials);
    mats.Add(mat);
    renderer.materials = mats.ToArray();
  }

  static void ActivateHighlight(Renderer renderer, float outlineWidth)
  {
    renderer.materials[renderer.materials.Length - 1].SetFloat("_Outline", outlineWidth);
  }

  static void DeactivateHighlight(Renderer renderer)
  {
    renderer.materials[renderer.materials.Length - 1].SetFloat("_Outline", 0.0f);
  }

  public static void ToggleHighlight(Renderer renderer, bool active)
  {
    if (active)
      ActivateHighlight(renderer, 0.03f);
    else
      DeactivateHighlight(renderer);
  }
}