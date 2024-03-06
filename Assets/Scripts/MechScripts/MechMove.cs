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
    private bool wantToJump;

    [Header("Strafing")]
    [SerializeField] private float MoveSpeed;
    private float MaxMoveSpeed;
    [SerializeField] private float strafeMoveSpeed;
    [SerializeField] private float maxGroundAccel;
    [SerializeField] private float maxAirAccel;

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
    private Vector3 move3d;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // normalize input
        _MoveMode = MoveMode.Default;
        strafeThrusterActive = false;
        wantToJump = false;
        MaxMoveSpeed = MoveSpeed;
    }

    #region IMoveInput

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        move3d = new Vector3(move.x, 0f, move.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        wantToJump = context.ReadValueAsButton();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (_MoveMode == MoveMode.Default || _MoveMode == MoveMode.StrafeThrusters)
        {
            if (_thrustCapacity > dashCost)
            {
                // start dash
                _MoveMode = MoveMode.Dash;
                dashDir = transform.TransformDirection(move3d);
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
        if (_MoveMode == MoveMode.Default || _MoveMode == MoveMode.StrafeThrusters)
        {
            if (_rushCapacity > 0f)
            {
                // start dash
                _rushDelay = rushDelay;
                _MoveMode = MoveMode.RushThrusters;
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
        switch (_MoveMode)
        {
            case MoveMode.Default:
            case MoveMode.StrafeThrusters:
                LateralMove();
                if (wantToJump) VerticalMove();
                break;
            case MoveMode.RushThrusters:
                RushThrust();
                break;
            case MoveMode.Dash:
                Dash();
                break;
        }
    }

    private void Update()
    {
        ThrusterUpdate();
    }

    #region movement

    private void LateralMove()
    {
        // find current and target velocities
        Vector3 currentVelocity = new Vector3(cc.velocity.x,0f,cc.velocity.z);
        Vector3 adjustedMove3d = transform.TransformDirection(move3d);
        Vector3 targetVelocity = adjustedMove3d * (strafeThrusterActive? strafeMoveSpeed : MoveSpeed);

        // difference in velocities
        Vector3 velDiff = targetVelocity - currentVelocity;

        // initialize acceleration
        Vector3 lateralMove;

        // evaluate based on grounded and angle between velocities
        if (strafeThrusterActive)
        {
            
        }
        else
        {
            if (isGrounded)
            {
                // accelerate to target velocity
                lateralMove = velDiff.normalized * maxGroundAccel * Time.fixedDeltaTime;
                lateralMove = Vector3.ClampMagnitude(lateralMove + currentVelocity, MoveSpeed);
                cc.Move(lateralMove * Time.fixedDeltaTime);
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
                cc.Move(lateralMove);
            }
        }
    }

    private void VerticalMove()
    {
        // only jump when grounded
        if (isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            // Only use below if using character controller
            cc.Move(jumpVelocity * Vector3.up);
        }
        else if (_thrustDelay <= 0f && cc.velocity.y < maxThrustVelocity && _thrustCapacity > 0f)
        {
            _thrustDelay = thrustDelay;
            if (_thrustCapacity > 0f) _thrustCapacity += Time.fixedDeltaTime;

            float vertThrust = maxThrustAccel * Time.fixedDeltaTime;
            Vector3 vertThrustVelChange = new Vector3(0, vertThrust, 0);

            cc.Move(vertThrustVelChange);
        }
    }

    private void Gravity()
    {
        if (isGrounded)
        {
            // push down to stick to surface
            cc.Move(Vector3.down * 5f);
        }
        else
        {
            // push down on player while in the air
            cc.Move(gravity * Vector3.down * Time.fixedDeltaTime * Time.fixedDeltaTime);
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

public enum MoveMode
{
    Default,
    StrafeThrusters,
    RushThrusters,
    Dash,
    Size
}