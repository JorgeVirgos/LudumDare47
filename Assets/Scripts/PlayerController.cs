using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventorySystem))]
public class PlayerController : MonoBehaviour
{

  public float CameraSpeedX = 1.0f;
  public float CameraSpeedY = 1.0f;
  public float MinCameraAngleY = -60.0f;
  public float MaxCameraAngleY = 60.0f;
  public float PlayerSpeed = 100.0f;
  public float JumpForce = 100.0f;
  public float JumpRaycastDistance = 0.6f;
  public float InteractableRaycastDistance = 100.0f;
  public Camera FPSCamera;
  public KeyCode InteractKey = KeyCode.E;
  public KeyCode ReloadKey = KeyCode.R;

  private InventorySystem Inventory;
  private Weapon CurrentWeapon;
  private float horizontalInput;
  private float verticalInput;
  private Rigidbody playerRB;
  private Vector3 movementDir;

  // Start is called before the first frame update
  void Start()
  {
    // FPSCamera = GetComponent<Camera>();
    playerRB = GetComponent<Rigidbody>();

    horizontalInput = transform.eulerAngles.y;
    verticalInput = transform.eulerAngles.x;

    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    Inventory = GetComponent<InventorySystem>();
    InventoryItem Item = Inventory.GetInventoryItemByIndex(InventoryItem.ItemType.kItemTypeWeapon, 0);
    if (Item)
    {
      CurrentWeapon = (Weapon)Item;
    }
    /*if(!Inventory) {
      Instantiate();
    }*/

    movementDir = Vector3.zero;
  }

  void SwapWeapon(int weaponIndex)
  {
    InventoryItem Item = Inventory.GetInventoryItemByIndex(InventoryItem.ItemType.kItemTypeWeapon, weaponIndex);
    if (Item)
    {
      if (Item.isPicked)
      {
        CurrentWeapon.gameObject.SetActive(false);
        CurrentWeapon = (Weapon)Item;
        CurrentWeapon.gameObject.SetActive(true);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    horizontalInput = Input.GetAxis("Mouse X") * CameraSpeedX * Time.deltaTime;
    verticalInput += Input.GetAxis("Mouse Y") * CameraSpeedY * Time.deltaTime;

    verticalInput = Mathf.Clamp(verticalInput, MinCameraAngleY, MaxCameraAngleY);

    FPSCamera.transform.eulerAngles =
      new Vector3(-verticalInput, FPSCamera.transform.eulerAngles.y, 0.0f);

    transform.Rotate(0.0f, horizontalInput, 0.0f);

    Vector3 a = Input.GetAxis("Horizontal") * transform.right;
    Vector3 b  = Input.GetAxis("Vertical") * transform.forward;
    movementDir = a + b;

    movementDir.Normalize();

    
    //movementDir = 

    /*
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0.0f, 0.0f, MovementFactor);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(-MovementFactor, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(0.0f, 0.0f, -MovementFactor);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(MovementFactor, 0.0f, 0.0f);
            */
    if (Input.GetAxis("Fire1") > 0.0f)
    {
      CurrentWeapon.Shoot();
    }
    else
    {
      CurrentWeapon.bIsShooting = false;
    }

    if (Input.GetKey(KeyCode.Q))
    {
      SwapWeapon(0);
    }

    if (Input.GetKey(KeyCode.T))
    {
      SwapWeapon(1);
    }

    if (Input.GetKey(KeyCode.Y))
    {
      SwapWeapon(2);
    }

    if (Input.GetKey(ReloadKey))
    {
      CurrentWeapon.Reload();
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      RaycastHit hit;
      Ray downRay = new Ray(transform.position, -Vector3.up);
      Physics.Raycast(downRay, out hit);
      if (hit.distance <= JumpRaycastDistance)
      {
        playerRB.AddForce(new Vector3(0.0f, JumpForce, 0.0f),
          ForceMode.Impulse);
      }
    }

    RaycastHit InteractableHit;
    Ray interactableRay = new Ray(transform.position, FPSCamera.transform.forward);
    Physics.Raycast(interactableRay, out InteractableHit);
    if (InteractableHit.distance <= InteractableRaycastDistance)
    {
      if (Input.GetKey(InteractKey))
      {
        // Call function
      }
    }

  }

  private void FixedUpdate()
  {
    Vector3 totalForce = movementDir * PlayerSpeed * Time.fixedDeltaTime;
    playerRB.AddForce(totalForce, ForceMode.VelocityChange);
  }
}
