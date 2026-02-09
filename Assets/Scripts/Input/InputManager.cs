using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem playerInput;
    private InputSystem.PlayerActions playerActions;
    private PlayerMotor motor;
    private PlayerLook look;
    void Awake()
    {
        playerInput = new InputSystem();
        playerActions = playerInput.Player;
        
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        
        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Sprint.started += ctx => motor.StartSprint();
        playerActions.Sprint.canceled += ctx => motor.EndSprint();
    }
    
    void FixedUpdate()
    {
        motor.ProcessMove(playerActions.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(playerActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
