using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    
    [SerializeField]
    private bool canMove = true;
    
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    
    
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
