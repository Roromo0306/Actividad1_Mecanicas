using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float gravity = 30f;

    [Header("Look Movement")]
    [SerializeField,Range(1,10)] private float lookSpeedX = 2f;
    [SerializeField,Range(1,10)] private float lookSpeedY = 2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;

    public void Awake()
    {
        playerCamera = GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(walkSpeed = Input.GetAxisRaw("Vertical"), walkSpeed * Input.GetAxisRaw("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;

        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
