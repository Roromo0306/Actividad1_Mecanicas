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
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    [Header("Head Bob Settings")]
    [SerializeField] private float headBobAmount = 0.05f; 
    [SerializeField] private float headBobSpeed = 10f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;

    private Vector3 defaultCameraPosition;
    private float headBobTimer = 0f;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultCameraPosition = playerCamera.transform.localPosition;
    }

    private void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            ApplyFinalMovements();
            HandleHeadBob();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        currentInput *= walkSpeed;

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.forward * currentInput.x) + (transform.right * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -lowerLookLimit, upperLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeedX, 0f);
    }

    private void ApplyFinalMovements()
    {
        if (characterController.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -1f;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleHeadBob()
    {
        if (characterController.velocity.magnitude > 0.1f && characterController.isGrounded)
        {
            headBobTimer += Time.deltaTime * headBobSpeed;
            float bobOffset = Mathf.Sin(headBobTimer) * headBobAmount;
            playerCamera.transform.localPosition = defaultCameraPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            
            headBobTimer = 0f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, defaultCameraPosition, Time.deltaTime * headBobSpeed);
        }
    }
}