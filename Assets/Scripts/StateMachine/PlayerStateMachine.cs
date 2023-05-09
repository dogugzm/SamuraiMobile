using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInput playerInput;

    Vector2 currentMovementInput;

    Vector3 currentMovement;
    Vector3 appliedMovement;

    bool isMovementPressed;
    bool isRunPressed;

    float runMultiplier = 3f;

    PlayerBaseState currentState;
    PlayerStateFactory states;

    #region Getter-Setters
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }

    public bool IsMovementPressed { get => isMovementPressed; set => isMovementPressed = value; }
    public bool IsRunPressed { get => isRunPressed; set => isRunPressed = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public int IsWalkingHash { get => isWalkingHash; set => isWalkingHash = value; }
    public int IsRunningHash { get => isRunningHash; set => isRunningHash = value; }
    public Vector3 CurrentMovement { get => currentMovement; set => currentMovement = value; }
    public float CurrentMovementY { get => currentMovement.y; set => currentMovement.y = value; }

    public Vector3 AppliedMovement { get => appliedMovement; set => appliedMovement = value; }
    public float AppliedMovementY { get => appliedMovement.y; set => appliedMovement.y = value; }
    public Vector2 CurrentMovementInput { get => currentMovementInput; set => currentMovementInput = value; }
    public float RunMultiplier { get => runMultiplier; set => runMultiplier = value; }

    #endregion

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

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.performed += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;

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
        currentState.UpdateStates();

        if (isRunPressed)
        {
            appliedMovement.x = currentMovement.x * runMultiplier;
            appliedMovement.z = currentMovement.z * runMultiplier;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;

        }

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }


    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
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
