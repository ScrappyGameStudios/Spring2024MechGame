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
    [SerializeField] private Collider PlayerCollider;
    private bool isGrounded;
    private bool strafeThrusterActive;
    private bool rushThrusterActive;
    private bool dashActive;
    private bool wantToJump;
    private bool wantToRush;

    [Header("Strafing")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float strafeMoveSpeed;
    [SerializeField] private float maxGroundForce;
    [SerializeField] private float maxAirForce;

    [Header("Vertical Thruster")]
    [SerializeField] private float maxThrustForce;
    [SerializeField] private float maxThrustVelocity;
    [SerializeField] private float thrustRechargeRate;
    [SerializeField] private float thrustDelay;
    [SerializeField] private float thrustCapacity;
    private float _thrustDelay;
    private float _thrustCapacity;

    [Header("Rush Thrusters")]
    [SerializeField] private float ThrustSpeed;

    [Header("Air Movement")]
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
        wantToJump = context.ReadValueAsButton();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Dash();
    }

    public void OnRushThrust(InputAction.CallbackContext context)
    {
        wantToRush = context.ReadValueAsButton();
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

    private void Update()
    {
        ThrusterUpdate();
    }

    #region movement

    // lateral move
    private void LateralMove()
    {
        // check if grounded
        // raycast

        // find target velocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= strafeThrusterActive? strafeMoveSpeed : moveSpeed;

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

            rb.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }
        else
        {
            if (rb.velocity.y < maxThrustVelocity && _thrustCapacity > 0f)
            {
                _thrustDelay = thrustDelay;
                if (_thrustCapacity > 0f) _thrustCapacity += Time.fixedDeltaTime;

                float vertThrust = maxThrustForce * Time.fixedDeltaTime;
                Vector3 vertThrustVelChange = new Vector3(0, vertThrust, 0);

                rb.AddForce(vertThrustVelChange, ForceMode.VelocityChange);
            }
        }
    }

    private void ThrusterUpdate()
    {
        // thruster stuff
        if (_thrustDelay > 0f) _thrustDelay -= Time.deltaTime;
        else if (_thrustCapacity > 0f) _thrustCapacity += (thrustRechargeRate * Time.deltaTime);
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