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
  public KeyCode ReloadKey = KeyCode.R;
  [Range(0.01f, 1.0f)]
  public float jumpFix = 0.05f;

  private InventorySystem Inventory;
  private Weapon CurrentWeapon;
  private float horizontalInput;
  private float verticalInput;
  private Rigidbody playerRB;
  private Vector3 movementDir;
  private bool shouldJump;
  private bool isJumping;

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
      shouldJump = true;
    }
  }

  private void FixedUpdate()
  {
    //Jump
    RaycastHit hit;
    // Does the ray intersect any objects excluding the player layer
    Vector3 dir = -Vector3.up;
    Vector3 initPos = transform.position;
    isJumping = !Physics.Raycast(initPos, dir, out hit, JumpRaycastDistance);

    if (shouldJump && !isJumping)
    {
      int mask = ~LayerMask.GetMask("PlayerTrigger");

      if (Physics.Raycast(initPos, dir, out hit, JumpRaycastDistance, mask))
      {
        playerRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
      }
      shouldJump = false;
    }

    // Movement
    jumpFix = isJumping ? 0.01f : 1.0f;
    Vector3 totalForce = movementDir * PlayerSpeed * jumpFix;
    playerRB.AddForce(totalForce, ForceMode.VelocityChange);
  }
}
