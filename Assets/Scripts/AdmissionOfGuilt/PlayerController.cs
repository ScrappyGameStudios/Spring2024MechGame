using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 0.5f;
    [SerializeField]
    private float gravityValue = -10f;
    [SerializeField]
    private float rotationSpeed = 0.95f;
    [SerializeField]
    private float accelRate = 0.2f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private Vector2 moveInput;
    private bool wantToJump;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    #region input
    public void OnMove(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>(); }
    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed) wantToJump = true;
        else if (context.canceled) wantToJump = false;
    }
    #endregion

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Move();
    }


    private void Move()
    {
        // check if on the ground
        groundedPlayer = controller.isGrounded;

        // jump, damn you!
        if (groundedPlayer)
        {
            if (playerVelocity.y < 0f) playerVelocity.y = -5f;
            if (wantToJump) playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        Vector3 lateralMove = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
        lateralMove = moveInput.x * cameraTransform.right.normalized + moveInput.y * lateralMove;

        Vector2 latMove = new Vector2(lateralMove.x, lateralMove.z);
        currentInputVector = Vector2.SmoothDamp(currentInputVector, latMove, ref smoothInputVelocity, accelRate);
        Vector3 move = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        move *= Time.deltaTime * playerSpeed;

        // perform gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(move + playerVelocity * Time.deltaTime);

        // rotate towards camera direction
        Quaternion rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }
}

public enum MoveMode
{
    Default,
    RushThrusters,
    Dash,
    Size
}