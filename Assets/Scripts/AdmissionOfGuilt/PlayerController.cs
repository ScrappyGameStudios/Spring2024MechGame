using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region MoveMode variables
    [Header("Starting MoveMode")]
    [SerializeField]
    private MoveMode playerMoveMode = MoveMode.Standard;

    [Header("MoveMode Speed")]
    [SerializeField]
    private float standardSpeed = 5.0f;
    [SerializeField]
    private float caprineSpeed = 6.0f;
    [SerializeField]
    private float strafeThrusterSpeed = 12.0f;

    private float playerSpeed;

    [Header("MoveMode Acceleration")]
    [SerializeField]
    private float standardGroundRate = 0.2f;
    [SerializeField]
    private float caprineGroundRate = 0.05f;
    [SerializeField]
    private float standardAirRate = 0.5f;
    [SerializeField]
    private float caprineAirRate = 0.5f;
    [SerializeField]
    private float strafeThrusterRate = 0.35f;

    private float groundAccelRate;
    private float airAccelRate;

    [Header("MoveMode Jump Height")]
    [SerializeField]
    private float standardJumpHeight = 2.0f;
    [SerializeField]
    private float caprineJumpHeight = 5.0f;
    [SerializeField]
    private float strafeThrusterJumpHeight = 3.0f;

    private float jumpHeight;

    [Header("MoveMode Gravity")]
    [SerializeField]
    private float standardGravity = -10.0f;
    [SerializeField]
    private float caprineGravity = -10.0f;
    [SerializeField]
    private float strafeThrusterGravity = -8.0f;

    private float gravityValue;

    [Header("MoveMode Rotation Speed")]
    [SerializeField]
    private float standardRotation = 0.95f;
    [SerializeField]
    private float caprineRotation = 1f;
    [SerializeField]
    private float strafeThrusterRotation = 0.75f;

    private float rotationSpeed;

    [Header("MoveMode Slope Angle")]
    [SerializeField]
    private float standardSlope = 45f;
    [SerializeField]
    private float caprineSlope = 70f;
    [SerializeField]
    private float strafeThrusterSlope = 45f;

    private float slopeAngle;

    [Header("MoveMode Slope Angle")]
    [SerializeField]
    private float standardStep = 45f;
    [SerializeField]
    private float caprineStep = 70f;
    [SerializeField]
    private float strafeThrusterStep = 45f;

    private float stepHeight;
    #endregion MoveMode variables

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private Vector2 moveInput;
    private Vector2 aimInput;
    private bool wantToJump;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private MoveAbility currentAbility;
    private Vector3 DashDir;

    #region Input
    public void OnMove(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>(); }
    public void OnJump(InputAction.CallbackContext context) { wantToJump = context.ReadValueAsButton(); }
    public void OnAim(InputAction.CallbackContext context) { aimInput = context.ReadValue<Vector2>(); }
    #endregion Input

    private void Start()
    {
        currentAbility = MoveAbility.None;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // set to standard mode by default
        SetMoveMode(playerMoveMode);
    }

    void Update()
    {
        switch (currentAbility)
        {
            case MoveAbility.Dash:
                Dash();
                break;
            case MoveAbility.Rush:
                Rush();
                break;
            default:
                Move();
                break;
        }
    }

    public void SetMoveMode(MoveMode Mode)
    {
        // set mode
        playerMoveMode = Mode;

        // set all variables
        switch (playerMoveMode)
        {
            case MoveMode.Standard:
                groundAccelRate = standardGroundRate;
                airAccelRate = standardAirRate;
                playerSpeed = standardSpeed;
                jumpHeight = standardJumpHeight;
                gravityValue = standardGravity;
                rotationSpeed = standardRotation;
                slopeAngle = standardSlope;
                stepHeight = standardStep;
                break;
            case MoveMode.CaprineAscenders:
                groundAccelRate = caprineGroundRate;
                airAccelRate = caprineAirRate;
                playerSpeed = caprineSpeed;
                jumpHeight = caprineJumpHeight;
                gravityValue = caprineGravity;
                rotationSpeed = caprineRotation;
                slopeAngle = caprineSlope;
                stepHeight = caprineStep;
                break;
            case MoveMode.StrafeThrusters:
                groundAccelRate = strafeThrusterRate;
                airAccelRate = strafeThrusterRate;
                playerSpeed = strafeThrusterSpeed;
                jumpHeight = strafeThrusterJumpHeight;
                gravityValue = strafeThrusterGravity;
                rotationSpeed = strafeThrusterRotation;
                slopeAngle = strafeThrusterSlope;
                stepHeight = strafeThrusterStep;
                break;
        }

        // properly set step and slope
        controller.slopeLimit = slopeAngle;
        controller.stepOffset = stepHeight;
    }
    public void SetMoveAbility(MoveAbility Ability) 
    { 
        currentAbility = Ability; 
        switch (currentAbility)
        {
            case MoveAbility.Dash:
                StartDash();
                break;
            case MoveAbility.Rush:
                StartRush();
                break;
        }
    }

    private void Move()
    {
        // initialize acceleration and such
        float accel = groundAccelRate;

        // check if on the ground
        groundedPlayer = controller.isGrounded;

        // jump, damn you!
        if (groundedPlayer)
        {
            // vertical velocity
            if (playerVelocity.y < 0f) playerVelocity.y = -5f;
            if (wantToJump) playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        else
        {
            accel = airAccelRate;
        }

        Vector3 lateralMove = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
        lateralMove = moveInput.x * cameraTransform.right.normalized + moveInput.y * lateralMove;

        Vector2 latMove = new Vector2(lateralMove.x, lateralMove.z);
        currentInputVector = Vector2.SmoothDamp(currentInputVector, latMove, ref smoothInputVelocity, accel);
        Vector3 move = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        move *= Time.deltaTime * playerSpeed;

        // perform gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(move + playerVelocity * Time.deltaTime);

        // rotate towards camera direction
        Quaternion rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }

    private void Dash()
    {

    }
    private void StartDash() 
    {
        DashDir = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
        DashDir = moveInput.x * cameraTransform.right.normalized + moveInput.y * DashDir;
    }

    private void Rush()
    {

    }
    private void StartRush() { }
}

public enum MoveMode
{
    Standard,
    CaprineAscenders,
    StrafeThrusters,
    Size
}
public enum MoveAbility
{
    None,
    Dash,
    Rush,
    Cooldown,
    Size
}