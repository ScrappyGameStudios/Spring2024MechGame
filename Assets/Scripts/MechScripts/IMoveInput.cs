using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMoveInput
{
    void OnMove(InputAction.CallbackContext context);
    void OnJump(InputAction.CallbackContext context);
    void OnDash(InputAction.CallbackContext context);
    void OnRushThrust(InputAction.CallbackContext context);
    void OnToggleStrafeThrusters(InputAction.CallbackContext context);
    void SetGrounded(bool state);
    void SetMaxMoveSpeed(float newMoveSpeed);
    void ResetMaxMoveSpeed();
    GameObject GetGameObject();
    void DebugCheck();
}
