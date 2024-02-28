using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputAcceptor
{
    void OnMouseLook(InputAction.CallbackContext context);
    void OnGamepadLook(InputAction.CallbackContext context);
    void OnTarget(InputAction.CallbackContext context);
    void OnRightAttack(InputAction.CallbackContext context);
    void OnRightReload(InputAction.CallbackContext context);
    void OnLeftAttack(InputAction.CallbackContext context);
    void OnLeftReload(InputAction.CallbackContext context);
    void OnRightShoulder(InputAction.CallbackContext context);
    void OnLeftShoulder(InputAction.CallbackContext context);
    void OnMelee(InputAction.CallbackContext context);
    void OnMove(InputAction.CallbackContext context);
    void OnJump(InputAction.CallbackContext context);
    void OnDash(InputAction.CallbackContext context);
    void OnRushThrust(InputAction.CallbackContext context);
    void OnToggleStrafeThrusters(InputAction.CallbackContext context);
    void SetGrounded(bool state);
    GameObject GetGameObject();
    void DebugCheck();
}
