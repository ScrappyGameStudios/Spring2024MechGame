using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechMove : MonoBehaviour, IMoveInput
{

    [Header("Important Components")]
    [SerializeField] private CharacterController cc;
    [SerializeField] private Animator MechMoveAnimator;
    [SerializeField] private Transform CameraParent;
    [SerializeField] private Collider GroundCheck;
    [SerializeField] private Collider PlayerCollider;
    private MoveMode _MoveMode;
    private bool isGrounded;
    private bool strafeThrusterActive;
    private bool caprineAscendersActive;
    private bool wantToJump;

    [Header("Strafing")]
    [SerializeField] private float MoveSpeed;
    private float MaxMoveSpeed;
    [SerializeField] private float strafeMoveSpeed;
    [SerializeField] private float maxStrafeAccel;
    [SerializeField] private float maxCaprineAccel;
    [SerializeField] private float maxGroundAccel;
    [SerializeField] private float maxAirAccel;
    private Vector3 move;

    [Header("Vertical Thruster")]
    [SerializeField] private float maxThrustAccel;
    [SerializeField] private float maxThrustVelocity;
    [SerializeField] private float thrustRechargeRate;
    [SerializeField] private float thrustDelay;
    [SerializeField] private float thrustCapacity;
    [SerializeField] private float thrustRechargeDelay;
    [SerializeField] private float dashCost;
    private Vector3 dashDir;
    private float _thrustDelay;
    private float _thrustCapacity;
    private float _thrustRechargeDelay;

    [Header("Rush Thrusters")]
    [SerializeField] private float rushSpeed;
    [SerializeField] private float rushDelay;
    [SerializeField] private float rushCapacity;
    private float _rushDelay;
    private float _rushCapacity;

    [Header("Air Movement")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    // input variables
    private Vector2 moveInput;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // normalize input
        _MoveMode = MoveMode.Standard;
        strafeThrusterActive = false;
        wantToJump = false;
        MaxMoveSpeed = MoveSpeed;
    }

    #region IMoveInput

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        wantToJump = context.ReadValueAsButton();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (_MoveMode == MoveMode.Standard)
        {
            if (_thrustCapacity > dashCost)
            {
                // start dash
                dashDir = transform.TransformDirection(moveInput);
                _thrustCapacity = Mathf.Clamp(_thrustCapacity - dashCost, 0f, _thrustCapacity);
            }
            else
            {
                // play sound
            }
        }
    }

    public void OnRushThrust(InputAction.CallbackContext context)
    {
        if (_MoveMode == MoveMode.Standard)
        {
            if (_rushCapacity > 0f)
            {
                // start dash
                _rushDelay = rushDelay;
                _MoveMode = MoveMode.StrafeThrusters;
            }
            else
            {
                // play sound
            }
        }
    }

    public void OnToggleStrafeThrusters(InputAction.CallbackContext context)
    {
        if (context.performed) StrafeThrusterToggle();
    }

    public void SetGrounded(bool state) { isGrounded = state; }
    
    public void SetMaxMoveSpeed(float newMoveSpeed)
    {
        MoveSpeed = newMoveSpeed;
    }
    
    public void ResetMaxMoveSpeed() { MoveSpeed = MaxMoveSpeed; }

    public GameObject GetGameObject() { return gameObject; }

    public void DebugCheck()
    {
        Debug.Log("MechMove found!");
    }

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        // movement
        move = Vector3.zero;
        switch (_MoveMode)
        {
            case MoveMode.Standard:
                LateralMove();
                Gravity();
                if (wantToJump) VerticalMove();
                break;
            case MoveMode.StrafeThrusters:
                RushThrust();
                break;
        }
        cc.Move(move);
    }

    private void Update()
    {
        ThrusterUpdate();
    }

    #region movement

    private void LateralMove()
    {
        float accel;
        if (strafeThrusterActive) accel = maxStrafeAccel;
        else if (isGrounded)
        {
            if (caprineAscendersActive) accel = maxCaprineAccel;
            else accel = maxGroundAccel;
        }
        else accel = maxAirAccel;

        moveInput = transform.TransformDirection(moveInput);
        currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInput, ref smoothInputVelocity, accel);
        Vector3 _move = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        move += _move * Time.fixedDeltaTime * (strafeThrusterActive? strafeMoveSpeed : MoveSpeed);

        /*
        // move += cc.velocity * Time.fixedDeltaTime;
        Debug.Log("LateralMove() called");
        // find current and target velocities
        Vector3 currentVelocity = new Vector3(cc.velocity.x,0f,cc.velocity.z);
        Vector3 adjustedMove3d = transform.TransformDirection(moveInput);
        Vector3 targetVelocity = adjustedMove3d * (strafeThrusterActive? strafeMoveSpeed : MoveSpeed);

        // difference in velocities
        Vector3 velDiff = targetVelocity - currentVelocity;

        // initialize acceleration
        Vector3 lateralMove;

        // evaluate based on grounded and angle between velocities
        if (strafeThrusterActive)
        {
            // accelerate to target velocity
            lateralMove = velDiff.normalized * maxThrustAccel * Time.fixedDeltaTime;
            lateralMove = Vector3.ClampMagnitude(lateralMove + currentVelocity, MoveSpeed);
            Debug.Log("lateralMove: " + lateralMove);
            move += lateralMove * Time.fixedDeltaTime;
        }
        else
        {
            if (isGrounded)
            {
                // accelerate to target velocity
                lateralMove = velDiff.normalized * maxGroundAccel * Time.fixedDeltaTime;
                lateralMove = Vector3.ClampMagnitude(lateralMove + currentVelocity, MoveSpeed);
                Debug.Log("lateralMove: " + lateralMove);
                move += lateralMove * Time.fixedDeltaTime;
            }
            else
            {
                // prevent (most) excess acceleration
                if (Vector3.Dot(adjustedMove3d, cc.velocity.normalized) > 0f && cc.velocity.magnitude < MoveSpeed)
                {
                    // remove component in direction of current velocity
                    adjustedMove3d -= cc.velocity.normalized * Vector3.Dot(adjustedMove3d, cc.velocity.normalized);
                }

                // accelerate in direction of input
                lateralMove = adjustedMove3d * maxAirAccel * Time.fixedDeltaTime * Time.fixedDeltaTime;
                lateralMove += cc.velocity * Time.fixedDeltaTime;
                Debug.Log("lateralMove: " + lateralMove);
                move += lateralMove;
            }
        }*/
    }

    private void VerticalMove()
    {
        // only jump when grounded
        if (isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            move += jumpVelocity * Vector3.up * Time.fixedDeltaTime;
        }
        else if (_thrustDelay <= 0f && cc.velocity.y < maxThrustVelocity && _thrustCapacity > 0f)
        {
            _thrustDelay = thrustDelay;
            if (_thrustCapacity > 0f) _thrustCapacity += Time.fixedDeltaTime;

            float vertThrust = maxThrustAccel * Time.fixedDeltaTime;
            Vector3 vertThrustVelChange = new Vector3(0, vertThrust, 0);

            move += vertThrustVelChange;
        }
    }

    private void Gravity()
    {
        Debug.Log("Gravity () called");
        if (isGrounded)
        {
            // push down to stick to surface
            move += Vector3.down * 5f;
        }
        else
        {
            // push down on player while in the air
            Vector3 gravityAccel = gravity * Vector3.down * Time.fixedDeltaTime * Time.fixedDeltaTime;
            Vector3 verticalVelocity = new Vector3(0f,cc.velocity.y * Time.fixedDeltaTime,0f);
            move += gravityAccel + verticalVelocity;
        }
    }

    private void ThrusterUpdate()
    {
        // thruster stuff
        if (!isGrounded)
        {
            // count down thrust delay when in the air
            _thrustDelay -= Time.deltaTime; 
        }
        else
        {
            // reset thrust when grounded
            _thrustDelay = thrustDelay;

            if (_thrustCapacity <= 0f)
            {
                // recharge thrust capacity while grounded
                _thrustCapacity += Time.deltaTime;
            }
        }
    }

    private void Dash()
    {

    }

    private void RushThrust()
    {
        // track both time and direction
    }

    private void StrafeThrusterToggle() { strafeThrusterActive = !strafeThrusterActive; }

    #endregion
}