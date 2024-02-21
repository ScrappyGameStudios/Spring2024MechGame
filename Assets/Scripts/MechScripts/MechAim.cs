using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechAim : MonoBehaviour, IAimInput
{
    [Header("Normal Necessary References")]
    [SerializeField] private Transform camHolder;

    [Header("Camera References")]
    [SerializeField] private Transform Cam;
    [SerializeField] private Vector3 DefaultCamPosition;
    [SerializeField] private Vector3 PrecisionCamPosition;
    [SerializeField] private Vector3 DashCamPosition;
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private Vector2 gamepadSensitivity;
    private Vector2 mouseLook, gamepadLook;
    private float lookRotation;
    private float gamepadAcceleration;
    private float gamepadFriction, gamepadMagnetism;
    private float mouseFriction, mouseMagnetism;
    private bool onRightSide;
    private Vector2 aimInput;

    [Header("Manual Aim")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxTurnMultiplier;
    [SerializeField] private float zoomedTurnMultiplier;
    [SerializeField] private float turnAcceleration;
    private float _turnSpeed;
    private bool isZoomed;

    #region IAimInput

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        gamepadLook = context.ReadValue<Vector2>();
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (context.performed) onRightSide = !onRightSide;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        GamepadLookAcceleration();
        AimMagnetism();
        Look();
    }

    private void Look()
    {
        // left/right inputs
        Vector3 mouseTurn = Vector3.up * mouseLook.x * mouseSensitivity.x * (1f - mouseFriction);
        Vector3 gamepadTurn = Vector3.up * gamepadLook.x * gamepadSensitivity.x * gamepadAcceleration * (1f - gamepadFriction);

        // turn
        camHolder.Rotate(mouseTurn + gamepadTurn);

        // up/down inputs
        float mouseRotation = -mouseLook.y * mouseSensitivity.y * (1f - mouseFriction);
        float gamepadRotation = -gamepadLook.y * gamepadSensitivity.y * gamepadAcceleration * (1f - gamepadFriction);

        // look
        lookRotation += (mouseRotation + gamepadRotation);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
    }

    /* Set up acceleration based on the magnitude and duration of gamepad aim input
     * Should not instantly reset when quickly changing from left to right or up to down and vice versa
     */
    private void GamepadLookAcceleration()
    {

    }

    public void AimMagnetism()
    {

    }

    public void UpdateAimAssist(float gpFriction, float gpMagnetism, float mFriction, float mMagnetism)
    {
        gamepadFriction = gpFriction;
        gamepadMagnetism = gpMagnetism;
        mouseFriction = mFriction;
        mouseMagnetism = mMagnetism;
    }
}
