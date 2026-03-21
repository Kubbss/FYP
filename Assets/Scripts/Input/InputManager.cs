using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem playerInput;
    public InputSystem.PlayerActions playerActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerSprint sprint;
    private PauseMenu pauseMenu;
    
    void Awake()
    {
        playerInput = new InputSystem();
        playerActions = playerInput.Player;
        
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        sprint = GetComponent<PlayerSprint>();
        
        pauseMenu = GameObject.FindWithTag("PlayerUI").GetComponent<PauseMenu>();
        
        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Sprint.started += ctx => sprint.TryStartSprint();
        playerActions.Sprint.canceled += ctx => sprint.TryEndSprint();
        playerActions.PauseMenu.performed += ctx => pauseMenu.TogglePause();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
