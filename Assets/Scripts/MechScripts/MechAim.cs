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
    private Camera camera;
    [SerializeField] private Transform Cam;
    [SerializeField] private Transform DefaultCamPosition;
    [SerializeField] private Transform PrecisionCamPosition;
    [SerializeField] private Vector3 DashCamPosition;
    [SerializeField] private Vector2 mouseDefaultSens;
    [SerializeField] private Vector2 gamepadDefaultSens;
    [SerializeField] private Vector2 mouseZoomSens;
    [SerializeField] private Vector2 gamepadZoomSens;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float aimedFOVMulti;
    [SerializeField] private float zoomSpeed;
    private float aimedFOV;
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
        if (context.performed)
        {
            if (isZoomed)
            {
                isZoomed = false;
                CamTarget = DefaultCamPosition;
                targetFOV = defaultFOV;
                mouseTargetSens = mouseDefaultSens;
                gamepadTargetSens = gamepadDefaultSens;
            }
            else
            {
                isZoomed = true;
                CamTarget = PrecisionCamPosition;
                targetFOV = aimedFOV;
                mouseTargetSens = mouseZoomSens;
                gamepadTargetSens = gamepadZoomSens;
            }

            Debug.Log("camera.fieldOfView: " + camera.fieldOfView);
            Debug.Log("targetFOV: " + targetFOV);

            Debug.Log("Cam Local Position: " + Cam.localPosition);
            Debug.Log("CamTarget Local Position: " + CamTarget.localPosition);
        }
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
        aimedFOV = defaultFOV / aimedFOVMulti;
        camera = Cam.GetComponent<Camera>();
        camera.fieldOfView = defaultFOV;
        targetFOV = defaultFOV;
        mouseSens = mouseDefaultSens;
        gamepadSens = gamepadDefaultSens;

        Debug.Log("defaultFOV: " + defaultFOV);
        Debug.Log("aimedFOV: " + aimedFOV);

        Debug.Log("Cam Local Position: " + Cam.localPosition);
        Debug.Log("CamTarget Local Position: " + CamTarget.localPosition);
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

        if (camera.fieldOfView != targetFOV)
        {
            // difference in FOV
            float zoomMagnitude = (targetFOV - camera.fieldOfView) / zoomSpeed * Time.deltaTime;

            camera.fieldOfView += zoomMagnitude;
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
