using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAimInput
{
    void OnAiming(InputAction.CallbackContext context);
    void OnTarget(InputAction.CallbackContext context);
}
