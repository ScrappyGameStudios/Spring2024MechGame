using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechAim : MonoBehaviour, IAimInput
{
    [Header("Normal Necessary References")]
    [SerializeField] private Transform CameraHolder;

    [Header("Camera Movement")]
    [SerializeField] private Transform Cam;
    [SerializeField] private Vector3 DefaultCamPosition;
    [SerializeField] private Vector3 PrecisionCamPosition;
    [SerializeField] private Vector3 DashCamPosition;
    private bool onRightSide;
    private Vector2 aimInput;

    #region IAimInput

    public void OnAiming(InputAction.CallbackContext context)
    {
        if (context.performed) aimInput = context.ReadValue<Vector2>();
        else if (context.canceled) aimInput = Vector2.zero;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
