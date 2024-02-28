using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAimInput
{
    void OnMouseLook(InputAction.CallbackContext context);
    void OnGamepadLook(InputAction.CallbackContext context);
    void OnTarget(InputAction.CallbackContext context);
    void OnZoomIn(float zoomLevel);
    void OnZoomOut();
    void DebugCheck();
}
