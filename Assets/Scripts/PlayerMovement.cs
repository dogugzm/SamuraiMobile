using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
     
    PlayerInput playerInput;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;

    bool isMovementPressed;
    bool isRunPressed;


    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    float rotationFactorPerFrame = 3f;


    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.performed += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;


    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }


    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        currentRunMovement.x = currentMovement.x * 3f;
        currentRunMovement.z = currentMovement.z * 3f;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

        }
        else if (!isMovementPressed && isWalking) {

            animator.SetBool(isWalkingHash, false);

        }

        if (isMovementPressed && isRunPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);

        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);

        }

    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float gravity = -0.05f;
            currentMovement.y = gravity;
            currentRunMovement.y = gravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y = gravity;
            currentRunMovement.y = gravity;
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;      

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleAnimation();
        HandleGravity();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);

        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);

        }


    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
