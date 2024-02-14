using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface ILoadoutInput
{
    void OnRightAttack(InputAction.CallbackContext context);
    void OnRightReload(InputAction.CallbackContext context);
    void OnLeftAttack(InputAction.CallbackContext context);
    void OnLeftReload(InputAction.CallbackContext context);
    void OnRightShoulder(InputAction.CallbackContext context);
    void OnLeftShoulder(InputAction.CallbackContext context);
    void OnMelee(InputAction.CallbackContext callback);
}
