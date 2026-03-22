using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 currentHorizontalVelocity;
    private bool isGrounded;
    
    
    [SerializeField] private bool canMove = true;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float acceleration = 18f;
    [SerializeField] private float airDeceleration = 3f;
    [SerializeField] private float groundedDeceleration = 25f;

    public Vector3 CurrentHorizontalVelocity => currentHorizontalVelocity;
    public bool IsGrounded => isGrounded;
    public bool CanMove => canMove;
    
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        if (!canMove)
            return;

        Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        Vector3 targetVelocity = transform.TransformDirection(inputDirection) * speed;

        float currentRate;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            currentRate = acceleration;
        }
        else
        {
            currentRate = isGrounded ? groundedDeceleration : airDeceleration;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            currentRate * Time.deltaTime
        );

        if (isGrounded && playerVelocity.y < 0f)
            playerVelocity.y = -2f;

        playerVelocity.y += gravity * Time.deltaTime;

        Vector3 finalVelocity = currentHorizontalVelocity;
        finalVelocity.y = playerVelocity.y;

        controller.Move(finalVelocity * Time.deltaTime);
    }
    
    /*
    public void ProcessMove(Vector2 input)
    {
        if (canMove)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
                playerVelocity.y = -2f;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
    */

    public void Jump()
    {
        if (isGrounded && canMove)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void StartSprint()
    {
        speed = 8f;
    }

    public void EndSprint()
    {
        speed = 5f;
    }

    public void LockPlayer()
    {
        canMove = false;
    }

    public void UnlockPlayer()
    {
        canMove = true;
    }
}
