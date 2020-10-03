using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
  public Weapon CurrentWeapon;

  private float horizontalInput;
  private float verticalInput;
  private Rigidbody playerRB;

  // Start is called before the first frame update
  void Start() {
    // FPSCamera = GetComponent<Camera>();
    playerRB = GetComponent<Rigidbody>();

    horizontalInput = transform.eulerAngles.y;
    verticalInput = transform.eulerAngles.x;

    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

  }

  // Update is called once per frame
  void Update() {
    horizontalInput = Input.GetAxis("Mouse X") * CameraSpeedX * Time.deltaTime;
    verticalInput += Input.GetAxis("Mouse Y") * CameraSpeedY * Time.deltaTime;

    verticalInput = Mathf.Clamp(verticalInput, MinCameraAngleY, MaxCameraAngleY);

    FPSCamera.transform.eulerAngles =
      new Vector3(-verticalInput, FPSCamera.transform.eulerAngles.y, 0.0f);

    transform.Rotate(0.0f, horizontalInput, 0.0f);

    float MovementFactor = PlayerSpeed * Time.deltaTime;

    if (Input.GetKey(KeyCode.W))
      transform.Translate(0.0f, 0.0f, MovementFactor);

    if (Input.GetKey(KeyCode.A))
      transform.Translate(-MovementFactor, 0.0f, 0.0f);

    if (Input.GetKey(KeyCode.S))
      transform.Translate(0.0f, 0.0f, -MovementFactor);

    if (Input.GetKey(KeyCode.D))
      transform.Translate(MovementFactor, 0.0f, 0.0f);

    if(Input.GetAxis("Fire1") > 0.0f) {
      CurrentWeapon.Shoot();
    } else {
      CurrentWeapon.bIsShooting = false;
    }

    if (Input.GetKey(ReloadKey)) {
      CurrentWeapon.Reload();
    }

    if (Input.GetKeyDown(KeyCode.Space)) {
      RaycastHit hit;
      Ray downRay = new Ray(transform.position, -Vector3.up);
      Physics.Raycast(downRay, out hit);
      if (hit.distance <= JumpRaycastDistance) {
        playerRB.AddForce(new Vector3(0.0f, JumpForce, 0.0f),
          ForceMode.Impulse);
      }
    }

    RaycastHit InteractableHit;
    Ray interactableRay = new Ray(transform.position, FPSCamera.transform.forward);
    Physics.Raycast(interactableRay, out InteractableHit);
    if (InteractableHit.distance <= InteractableRaycastDistance) {
      if(Input.GetKey(InteractKey)) {
        // Call function
      }
    }

  }
}
