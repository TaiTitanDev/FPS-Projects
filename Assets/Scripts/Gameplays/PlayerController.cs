using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmoothTime = 0.25f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;

    public float jumpSpeed = 16.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private bool isLockCursor;

    private float cameraPitch = 100.0f;
    private CharacterController characterController;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if(isLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MouseLook();
        Movement();
        Jump();
    }

    private void MouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void Movement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
