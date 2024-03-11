using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private float zoomTime;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas NormalFireCanvas;
    [SerializeField]
    private Canvas AimedFireCanvas;

    private float zoomLevel;

    private CinemachineVirtualCamera virtualCamera;

    #region public calls
    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed) StartAim();
        else if (context.canceled) StopAim();
    }
    public void OnModifyZoom(float zoom) { /*CineBrain = zoom;*/ }
    #endregion

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        AimedFireCanvas.enabled = true;
        NormalFireCanvas.enabled = false;
    }

    private void StopAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        NormalFireCanvas.enabled = true;
        AimedFireCanvas.enabled = false;
    }
}
