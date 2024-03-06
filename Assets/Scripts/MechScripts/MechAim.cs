using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechAim : MonoBehaviour, IAimInput
{
    [Header("Normal Necessary References")]
    [SerializeField] private Transform MechParent;

    [Header("Camera References")]
    [SerializeField] private Transform CamHolder;
    private Camera cam;
    [SerializeField] private Transform Cam;
    [SerializeField] private Transform DefaultCamPosition;
    [SerializeField] private Transform PrecisionCamPosition;
    [SerializeField] private Transform DashCamPosition;
    
    [Header("Sensitivity Defaults")]
    [SerializeField] private Vector2 mouseDefaultSens;
    [SerializeField] private Vector2 gamepadDefaultSens;
    [SerializeField] private float gamepadAccelRate;
    [SerializeField] private float gamepadAccelMax;

    [SerializeField] private float defaultFOV;
    [SerializeField] private float zoomSpeed;
    private float targetFOV;
    private Vector2 mouseLook, gamepadLook;
    private Vector2 mouseSens, gamepadSens;
    private Vector2 mouseTargetSens, gamepadTargetSens;
    private float lookRotation;
    private float gamepadAcceleration;
    private float gamepadFriction, gamepadMagnetism;
    private float mouseFriction, mouseMagnetism;
    private bool onRightSide;
    private Vector2 aimInput;
    private Transform CamTarget;
    private bool isLockOn;

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
        if (context.performed)
        {
            isLockOn = !isLockOn;
        }
    }

    public void OnZoomIn(float zoomLevel)
    {
        CamTarget = PrecisionCamPosition;
        targetFOV = defaultFOV / zoomLevel;
        mouseTargetSens = mouseDefaultSens / zoomLevel;
        gamepadTargetSens = gamepadDefaultSens / zoomLevel;
    }

    public void OnZoomOut()
    {
        CamTarget = DefaultCamPosition;
        targetFOV = defaultFOV;
        mouseTargetSens = mouseDefaultSens;
        gamepadTargetSens = gamepadDefaultSens;
    }

    public void DebugCheck()
    {
        Debug.Log("MechAim found!");
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mouseFriction = 0f;
        gamepadFriction = 0f;
        gamepadAcceleration = 1f;

        CamTarget = DefaultCamPosition;
        cam = Cam.GetComponent<Camera>();
        cam.fieldOfView = defaultFOV;
        targetFOV = defaultFOV;
        mouseTargetSens = mouseDefaultSens;
        gamepadTargetSens = gamepadDefaultSens;
    }

    private void LateUpdate()
    {
        GamepadLookAcceleration();
        AimMagnetism();
        Look();
        MoveCamera();
    }

    private void Look()
    {
        // left/right inputs
        Vector3 mouseTurn = Vector3.up * mouseLook.x * mouseSens.x * (1f - mouseFriction);
        Vector3 gamepadTurn = Vector3.up * gamepadLook.x * gamepadSens.x * gamepadAcceleration * (1f - gamepadFriction);

        // turn
        MechParent.Rotate(mouseTurn + gamepadTurn);

        // up/down inputs
        float mouseRotation = -mouseLook.y * mouseSens.y * (1f - mouseFriction);
        float gamepadRotation = -gamepadLook.y * gamepadSens.y * gamepadAcceleration * (1f - gamepadFriction);

        // look
        lookRotation += (mouseRotation + gamepadRotation);
        lookRotation = Mathf.Clamp(lookRotation, -75, 75);
        CamHolder.eulerAngles = new Vector3(lookRotation, CamHolder.eulerAngles.y, CamHolder.eulerAngles.z);
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

    private void MoveCamera()
    {
        if (Cam.localPosition != CamTarget.localPosition)
        {
            // get distance to move
            Vector3 camMagnitude = (CamTarget.localPosition - Cam.localPosition) / zoomSpeed * Time.deltaTime;

            Cam.localPosition += camMagnitude;
        }

        if (cam.fieldOfView != targetFOV)
        {
            // difference in FOV
            float zoomMagnitude = (targetFOV - cam.fieldOfView) / zoomSpeed * Time.deltaTime;

            cam.fieldOfView += zoomMagnitude;
        }

        if (mouseSens != mouseTargetSens || gamepadSens != gamepadTargetSens)
        {
            Vector2 mouseMagnitude = (mouseTargetSens - mouseSens) / zoomSpeed * Time.deltaTime;
            Vector2 gamepadMagnitude = (gamepadTargetSens - gamepadSens) / zoomSpeed * Time.deltaTime;

            mouseSens += mouseMagnitude;
            gamepadSens += gamepadMagnitude;
        }
    }
}
