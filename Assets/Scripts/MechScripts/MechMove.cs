using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechMove : MonoBehaviour, IMoveInput
{

    [Header("Important Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator MechMoveAnimator;
    [SerializeField] private Transform CameraParent;
    [SerializeField] private Collider GroundCheck;
    private bool isGrounded;
    private bool strafeThrusterActive;
    private bool rushThrusterActive;
    private bool dashActive;
    private bool wantToJump;

    [Header("Standard Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float thrustMoveSpeed;
    [SerializeField] private float maxGroundForce;
    [SerializeField] private float maxThrustForce;

    [Header("Rush Thrusters")]
    [SerializeField] private float ThrustSpeed;

    [Header("Air Movement")]
    [SerializeField] private float maxAirForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float landStallDuration;

    #region IMoveInput

    // input variables
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // normalize input
        move = Vector2.zero;
        strafeThrusterActive = false;
        rushThrusterActive = false;
        dashActive = false;
        wantToJump = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) wantToJump = true;
        else if (context.canceled) wantToJump = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) Dash();
    }

    public void OnRushThrust(InputAction.CallbackContext context)
    {
        if (context.performed) RushThrust();
    }

    public void OnToggleStrafeThrusters(InputAction.CallbackContext context)
    {
        if (context.performed) StrafeThrusterToggle();
    }
    
    public void ForceGrounded(bool state)
    {

    }

    public void SetGrounded(bool state) { isGrounded = state; }
    public GameObject GetGameObject() { return gameObject; }

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        // move or rush?
        if (rushThrusterActive) RushThrust();
        else LateralMove();

        if (wantToJump)
        {
            Jump();
        }
    }

    #region movement

    // lateral move
    private void LateralMove()
    {
        // find target velocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= strafeThrusterActive? thrustMoveSpeed : moveSpeed;

        // align direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        // calculate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        float maxMagnitude = isGrounded? maxGroundForce : maxAirForce;

        // limit force
        velocityChange = Vector3.ClampMagnitude(velocityChange, strafeThrusterActive? maxThrustForce : maxMagnitude);

        if (isGrounded || move.magnitude > 0f) rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // jump
    private void Jump()
    {
        if (isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            Vector3 newVertVelocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);

            rb.velocity = newVertVelocity;
        }
        else
        {
            float vertThrust = maxThrustForce * Time.fixedDeltaTime;
            Vector3 vertThrustVelChange = new Vector3(0, vertThrust, 0);

            rb.AddForce(vertThrustVelChange, ForceMode.VelocityChange);
        }

        
    }

    private void Dash()
    {

    }

    // rush thrusters
    private void RushThrust()
    {
        // track both time and direction
    }

    private void StrafeThrusterToggle()
    {
        strafeThrusterActive = !strafeThrusterActive;
    }

    #endregion
}